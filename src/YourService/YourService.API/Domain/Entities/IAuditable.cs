using YourBrand.YourService.API.Domain.ValueObjects;

namespace YourBrand.YourService.API.Domain.Entities;

public interface IAuditable
{
    DateTimeOffset Created { get; set; }
    UserId? CreatedById { get; set; }

    DateTimeOffset? LastModified { get; set; }
    UserId? LastModifiedById { get; set; }
}