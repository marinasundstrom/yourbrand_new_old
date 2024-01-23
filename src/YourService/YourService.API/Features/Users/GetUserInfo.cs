using FluentValidation;

using MediatR;

using YourBrand.YourService.API.Repositories;

namespace YourBrand.YourService.API.Features.Users;

public sealed record GetUserInfo() : IRequest<Result<UserInfoDto>>
{
    public sealed class Validator : AbstractValidator<GetUserInfo>
    {
        public Validator()
        {
        }
    }

    public sealed class Handler : IRequestHandler<GetUserInfo, Result<UserInfoDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly ICurrentUserService currentUserService;

        public Handler(IUserRepository userRepository, ICurrentUserService currentUserService)
        {
            this.userRepository = userRepository;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<UserInfoDto>> Handle(GetUserInfo request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindByIdAsync(currentUserService.UserId!, cancellationToken);

            if (user is null)
            {
                return Errors.Users.UserNotFound;
            }

            return user.ToDto2();
        }
    }
}