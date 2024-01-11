using FluentAssertions;

using NSubstitute;

using YourBrand.YourService.API.Domain.Events;
using YourBrand.YourService.API.Features.Todos;
using YourBrand.YourService.API.Persistence.Repositories.Mocks;
using YourBrand.YourService.API.Repositories;
using YourBrand.YourService.API.Services;

namespace YourBrand.Carts.UnitTests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        // Arrange

        var domainEventDispatcher = Substitute.For<IDomainEventDispatcher>();

        var unitOfWork = new MockUnitOfWork(domainEventDispatcher);
        var todosRepository = new MockTodoRepository(unitOfWork);

        var text = "Foo";

        var createTodo = new CreateTodo(text);
        var handler = new CreateTodo.Handler(todosRepository, unitOfWork);

        // Act

        var result = await handler.Handle(createTodo, default);

        // Assert

        result.GetValue().Text.Should().Be(text);

        await domainEventDispatcher
            .ReceivedWithAnyArgs()
            .Dispatch(Arg.Any<TodoCreated>(), default);
    }
}