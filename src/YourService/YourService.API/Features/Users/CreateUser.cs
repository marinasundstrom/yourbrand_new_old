using FluentValidation;

using MediatR;

using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Repositories;

namespace YourBrand.YourService.API.Features.Users;

public sealed record CreateUser(string Name, string Email) : IRequest<Result<UserInfoDto>>
{
    public class Validator : AbstractValidator<CreateUser>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(60);

            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public sealed class Handler : IRequestHandler<CreateUser, Result<UserInfoDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public Handler(IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<UserInfoDto>> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            string userId = currentUserService.UserId!;

            userRepository.Add(new User(userId, request.Name, request.Email));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var user = await userRepository.FindByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return Errors.Users.UserNotFound;
            }

            return user.ToDto2();
        }
    }
}