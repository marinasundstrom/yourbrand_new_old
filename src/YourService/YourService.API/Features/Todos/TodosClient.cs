using Microsoft.AspNetCore.SignalR;

namespace YourBrand.YourService.API.Features.Todos;

public interface ITodosClient
{
    Task TodoCreated(TodoDto todo);
}

public sealed class TodosClient(IHubContext<TodosHub, ITodosClient> hubContext) : ITodosClient
{
    public async Task TodoCreated(TodoDto todo)
    {
        await hubContext.Clients.All.TodoCreated(todo);
    }
}
