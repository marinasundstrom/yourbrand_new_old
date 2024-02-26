using System.Collections.Generic;

using Core;

using YourBrand.YourService.API;
using YourBrand.YourService.API.Domain.Events;
using YourBrand.YourService.API.Domain.ValueObjects;

namespace YourBrand.YourService.API.Domain.Entities;

public sealed class Todo : AggregateRoot<TodoId>, IAuditable, IHasTenant, ISoftDelete
{
    private Todo() : base()
    {
    }

    private Todo(string text) : base(new TodoId())
    {
        Text = text;
    }

    public string Text { get; set; }

    public bool IsCompleted { get; set; }

    public TenantId TenantId { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? CreatedBy { get; set; }

    public UserId? CreatedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public User? LastModifiedBy { get; set; }

    public UserId? LastModifiedById { get; set; }

    public DateTimeOffset? Deleted { get; set; }

    public UserId? DeletedById { get; set; }

    public static Todo Create(string text)
    {
        var todo = new Todo(text);
        todo.AddDomainEvent(new TodoCreated(todo.Id));
        return todo;
    }
}