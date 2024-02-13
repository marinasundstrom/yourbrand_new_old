using YourBrand.YourService.API.Domain.Entities;

namespace YourBrand.YourService.API.Features.Users;

public static class Mappings
{
    public static UserDto ToDto(this User user) => new(user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new(user.Id, user.Name);
}