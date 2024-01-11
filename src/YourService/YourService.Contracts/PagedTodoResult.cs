namespace YourBrand.YourService.Contracts;

public sealed record PagedTodoResult
{
    public IEnumerable<Todo> Items { get; init; }

    public int Total { get; init; }
}