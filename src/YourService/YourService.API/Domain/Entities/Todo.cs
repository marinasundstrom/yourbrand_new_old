using System.Collections.Generic;

using YourBrand.YourService.API;
using YourBrand.YourService.API.Domain.Events;
using YourBrand.YourService.API.Domain.ValueObjects;

using Core;

namespace YourBrand.YourService.API.Domain.Entities;

public class Todo : AggregateRoot<string>, IAuditable
{
    private Todo(string text) : base(Guid.NewGuid().ToString())
    {
        Text = text;
    }

    public string Text { get; set; }

    public bool IsCompleted { get; set; }

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public static Todo Create(string text)
    {
        var todo = new Todo(text);
        todo.AddDomainEvent(new TodoCreated(todo.Id));
        return todo;
    }
}