using Microsoft.AspNetCore.SignalR;

namespace YourBrand.YourService.API.Features.Todos;

public interface ITodosHub
{
}

public sealed class TodosHub : Hub<ITodosClient>, ITodosHub
{
}