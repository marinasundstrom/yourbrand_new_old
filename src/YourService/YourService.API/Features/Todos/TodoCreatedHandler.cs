using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API;
using YourBrand.YourService.API.Repositories;
using YourBrand.YourService.API.Common;
using YourBrand.YourService.API.Domain.Events;
using MassTransit;
using YourBrand.YourService.API.Domain.Entities;

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

        await todosClient.TodoCreated(todo.ToDto());

        await publishEndpoint.Publish(new Contracts.TodoCreated
        {
            TodoId = todo.Id
        });
    }
}
