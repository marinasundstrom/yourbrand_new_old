using System.Diagnostics.CodeAnalysis;

namespace YourBrand.YourService.API.Domain.ValueObjects;

public struct TodoId
{
    public TodoId(Guid value) => Value = value;

    public TodoId() => Value = Guid.NewGuid();

    public Guid Value { get; set; }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static bool operator ==(TodoId lhs, TodoId rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(TodoId lhs, TodoId rhs) => lhs.Value != rhs.Value;

    public static implicit operator TodoId(Guid id) => new TodoId(id);

    public static implicit operator TodoId?(Guid? id) => id is null ? (TodoId?)null : new TodoId(id.GetValueOrDefault());

    public static implicit operator Guid(TodoId id) => id.Value;
}