using YourBrand.YourService.API.Domain.ValueObjects;

namespace YourBrand.YourService.API.Domain.Entities;

public interface ISoftDelete
{
    UserId? DeletedById { get; set; }
    DateTimeOffset? Deleted { get; set; }
}