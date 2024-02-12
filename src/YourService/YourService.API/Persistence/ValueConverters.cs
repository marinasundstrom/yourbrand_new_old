using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using YourBrand.YourService.API.Domain.ValueObjects;

namespace YourBrand.YourService.API.Persistence;

internal sealed class TodoIdConverter : ValueConverter<TodoId, Guid>
{
    public TodoIdConverter()
        : base(v => v.Value, v => new(v))
    {
    }
}

internal sealed class UserIdConverter : ValueConverter<UserId, string>
{
    public UserIdConverter()
        : base(v => v.Value, v => new(v))
    {
    }
}

internal sealed class TenantIdConverter : ValueConverter<TenantId, string>
{
    public TenantIdConverter()
        : base(v => v.Value, v => new(v))
    {
    }
}