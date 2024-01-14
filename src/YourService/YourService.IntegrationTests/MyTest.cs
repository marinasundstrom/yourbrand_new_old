using MassTransit;
using MassTransit.Testing;

using Meziantou.Extensions.Logging.Xunit;

using Microsoft.AspNetCore.Mvc.Testing;

using YourBrand.YourService.Contracts;

using Xunit.Abstractions;
using System.Net.Http.Headers;

namespace YourBrand.YourService.IntegrationTests;

[Collection("Database collection")]
public class MyTest : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Func<Task> _resetDatabase;

    public HttpClient HttpClient { get; private set; }
    public ITestHarness Harness { get; private set; }

    public MyTest(YourServiceApiFactory factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory.WithTestLogging(testOutputHelper);

        _resetDatabase = factory.ResetDatabaseAsync;
    }

    public async Task InitializeAsync()
    {
        HttpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        Harness = _factory.Services.GetTestHarness();
        await Harness.Start();
    }

    [Fact]
    public async Task GetTodos()
    {
        // Arrange

        // Act
        var response = await HttpClient.GetAsync("/v1/todos");
        var str = await response.Content.ReadAsStringAsync();

        Console.WriteLine(str);

        // Assert
    }

    [Fact]
    public async Task CreateTodo()
    {
        // Arrange

        HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("TestScheme");

        // Act
        var response = await HttpClient.PostAsJsonAsync("/v1/todos", new { Text = "Test" });
        var str = await response.Content.ReadAsStringAsync();

        Console.WriteLine(str);

        // Assert
    }


    public async Task DisposeAsync() => await _resetDatabase();
}