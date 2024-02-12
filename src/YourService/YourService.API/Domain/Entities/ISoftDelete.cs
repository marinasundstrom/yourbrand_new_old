using YourBrand.YourService.API.Domain.ValueObjects;

namespace YourBrand.YourService.API.Domain.Entities;

public interface ISoftDelete
{
    DateTimeOffset? Deleted { get; set; }
    UserId? DeletedById { get; set; }
}