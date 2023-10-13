
using MassTransit.Testing;
using Carts.Contracts;
using Xunit.Abstractions;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.SqlEdge;
using Microsoft.EntityFrameworkCore;
using Carts.API.Data;
using MassTransit;

namespace Carts.IntegrationTests;
public class CartsTest : IAsyncLifetime
{
    ITestOutputHelper _testOutputHelper;
    private WebApplicationFactory<Program> factory;
    private HttpClient client;
    private ITestHarness harness;
    private const string CartsDbName = "yourbrand-carts-db";
    private const string DbServerName = "yourbrand-test-sqlserver";

    static readonly SqlEdgeContainer _sqlEdgeContainer = new SqlEdgeBuilder()
        .WithImage("mcr.microsoft.com/azure-sql-edge:1.0.7")
        .WithHostname(DbServerName)
        .WithName(DbServerName)
        .Build();

    public CartsTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public async Task InitializeAsync()
    {
        await _sqlEdgeContainer.StartAsync();

        factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => {
                builder.ConfigureLogging(x =>  {
                    x.ClearProviders();
                    x.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(_testOutputHelper));
                });

                builder.ConfigureServices(async services => {
                        var descriptor = services.Single(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<CartsContext>));

                        services.Remove(descriptor);

                        services.AddDbContext<CartsContext>((sp, options) =>
                        {
                            var connectionString = _sqlEdgeContainer.GetConnectionString().Replace("master", CartsDbName);
                            options.UseSqlServer(connectionString);
                        });

                        services.AddMassTransitTestHarness(x =>
                        {
                            x.AddDelayedMessageScheduler();
    
                            x.AddConsumers(typeof(Carts.API.Consumers.GetCartsConsumer).Assembly);
                            
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
                            var db = scopedServices.GetRequiredService<CartsContext>();
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
            });

        client = factory.CreateClient();

        harness = factory.Services.GetTestHarness();

        await harness.Start();
    }

    public async Task DisposeAsync()
    {
        await _sqlEdgeContainer.StopAsync()
        .ConfigureAwait(false);
    }

    [Fact]
    public async Task AddNewCartItem()
    {
        // Arrange
        
        var client = harness.GetRequestClient<AddCartItem>();

        // Act

        var response = await client.GetResponse<AddCartItemResponse>(
            new AddCartItem {
                CartId = "test",
                Name = "Test",
                ProductId = 100,
                ProductHandle = "foo",
                Description = "",
                Price = 20,
                Quantity = 1
            });

        // Assert

        Assert.True(await harness.Consumed.Any<AddCartItem>());

        Assert.True(await harness.Sent.Any<AddCartItemResponse>());
    }
}