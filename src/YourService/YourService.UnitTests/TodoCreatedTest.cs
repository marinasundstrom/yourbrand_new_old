using MassTransit;

using NSubstitute;

using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Domain.Events;
using YourBrand.YourService.API.Features.Todos;
using YourBrand.YourService.API.Repositories;

namespace YourBrand.YourService.UnitTests;

public class TodoCreatedTest
{
    [Fact]
    public async Task TodoCreated()
    {
        // Arrange

        var todoId = "Guid1";
        var todo = Todo.Create(todoId);

        var todosRepository = Substitute.For<ITodoRepository>();
        todosRepository
            .FindByIdAsync(Arg.Any<string>(), default)
            .ReturnsForAnyArgs(todo);

        var publishEndpoint = Substitute.For<IPublishEndpoint>();

        var todosClient = Substitute.For<ITodosClient>();

        var todoCreated = new TodoCreated(todoId);
        var handler = new TodoCreatedHandler(todosRepository, publishEndpoint, todosClient);

        // Act

        await handler.Handle(todoCreated, default);

        // Assert

        await todosRepository
            .ReceivedWithAnyArgs()
            .FindByIdAsync(Arg.Any<string>(), default);

        await todosClient
            .ReceivedWithAnyArgs()
            .TodoCreated(Arg.Any<TodoDto>());
    }
}