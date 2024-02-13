using Microsoft.AspNetCore.SignalR;

namespace YourBrand.YourService.API.Features.Todos;

public interface ITodosClient
{
    Task TodoCreated(TodoDto todo);

    Task TodoCompleted(string todoId);
}

public sealed class TodosClient(IHubContext<TodosHub, ITodosClient> hubContext) : ITodosClient
{
    public async Task TodoCreated(TodoDto todo)
    {
        await hubContext.Clients.All.TodoCreated(todo);
    }

    public async Task TodoCompleted(string todoId)
    {
        await hubContext.Clients.All.TodoCompleted(todoId);
    }
}