using YourBrand.YourService.API.Domain.Entities;

namespace YourBrand.YourService.API.Features.Todos;

public static class Mappings
{
    public static TodoDto ToDto(this Todo todo) => new(todo.Id, todo.Text);
}
