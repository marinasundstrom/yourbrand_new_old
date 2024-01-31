using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Domain.ValueObjects;

namespace YourBrand.YourService.API.Repositories;

public interface IUserRepository : IRepository<User, UserId>
{

}