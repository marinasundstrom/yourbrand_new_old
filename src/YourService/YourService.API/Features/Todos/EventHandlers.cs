using MassTransit;

using YourBrand.Domain;
using YourBrand.YourService.API.Domain.Events;
using YourBrand.YourService.API.Repositories;

namespace YourBrand.YourService.API.Features.Todos;

public sealed class TodoCreatedHandler(
    ITodoRepository todoRepository, IPublishEndpoint publishEndpoint, ITodosClient todosClient) : IDomainEventHandler<TodoCreated>
{
    public async Task Handle(TodoCreated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
        {
            throw new Exception();
        }

        await publishEndpoint.Publish(new Contracts.TodoCreated
        {
            TodoId = todo.Id.ToString()
        });

        await todosClient.TodoCreated(todo.ToDto());
    }
}

public sealed class TodoCompletedHandler(ITodosClient todosClient) : IDomainEventHandler<TodoCompleted>
{
    public async Task Handle(TodoCompleted notification, CancellationToken cancellationToken)
    {
        await todosClient.TodoCompleted(notification.TodoId.ToString());
    }
}