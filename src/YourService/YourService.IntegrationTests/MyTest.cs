using MassTransit;
using MassTransit.Testing;

using Meziantou.Extensions.Logging.Xunit;

using Microsoft.AspNetCore.Mvc.Testing;

using YourBrand.YourService.Contracts;

using Xunit.Abstractions;
using System.Net.Http.Headers;
using FluentAssertions;

namespace YourBrand.YourService.IntegrationTests;

[Collection("Database collection")]
public class MyTest : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Func<Task> _resetDatabase;

    public HttpClient HttpClient { get; private set; }
    public ITestHarness Harness { get; private set; }

    public MyTest(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper)
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

        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Act
        var response = await client.GetAsync("/v1/todos");
        var str = await response.Content.ReadAsStringAsync();

        Console.WriteLine(str);

        // Assert
    }

    [Fact]
    public async Task CreateTodo()
    {
        // Arrange

        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act
        var response = await client.PostAsJsonAsync("/v1/todos", new { Text = "Test" });
        var str = await response.Content.ReadAsStringAsync();

        Console.WriteLine(str);

        // Assert
    }

    [Fact]
    public async Task CreateTodo_Unauthorized_Fails()
    {
        // Arrange

        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        // Act
        Func<Task> act = async () =>
        {
            var response = await client.PostAsJsonAsync("/v1/todos", new { Text = "Test" });
            response.EnsureSuccessStatusCode();
        };

        // Assert

        await act.Should().ThrowAsync<HttpRequestException>();
    }


    public async Task DisposeAsync() => await _resetDatabase();
}