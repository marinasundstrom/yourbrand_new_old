
using DotNet.Testcontainers.Builders;

using MassTransit;
using MassTransit.Testing;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Respawn;

using YourBrand.YourService.API.Persistence;

using Testcontainers.SqlEdge;
using Microsoft.AspNetCore.Authentication;

namespace YourBrand.YourService.IntegrationTests;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string DbName = "yourbrand-service-db";
    private const string DbServerName = "yourbrand-test-sqlserver";
    static readonly SqlEdgeContainer _dbContainer = new SqlEdgeBuilder()
        .WithImage("mcr.microsoft.com/azure-sql-edge:1.0.7")
        .WithHostname(DbServerName)
        .WithName(DbServerName)
        .WithPortBinding(51736)
        .Build();
    private SqlConnection _dbConnection;
    private Respawner _respawner;

    public string DefaultUserId { get; set; } = "1";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(async services =>
        {
            var descriptor = services.Single(
        d => d.ServiceType ==
            typeof(DbContextOptions<ApplicationDbContext>));

            services.Remove(descriptor);

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var connectionString = _dbContainer.GetConnectionString().Replace("master", DbName);
                options.UseSqlServer(connectionString);
            });

            services.AddMassTransitTestHarness(x =>
            {
                x.AddDelayedMessageScheduler();

                x.AddConsumers(typeof(API.Features.Endpoints).Assembly);

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.UseDelayedMessageScheduler();

                    cfg.ConfigureEndpoints(context);
                });
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                var configuration = scopedServices.GetRequiredService<IConfiguration>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                try
                {
                    await Seed.SeedData(db, configuration);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        });

        builder.UseEnvironment("Development");

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication("TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "TestScheme", options => { });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        _dbConnection = new SqlConnection(_dbContainer.GetConnectionString().Replace("Database=master;", string.Empty));

        await InitializeRespawner();
    }

    public async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync()
        .ConfigureAwait(false);
    }
}
