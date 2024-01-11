using MediatR;

using YourBrand.YourService.API.Common;

namespace YourBrand.YourService.API.Features.Users;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        string GetOrdersExpire20 = nameof(GetOrdersExpire20);

        var versionedApi = app.NewVersionedApi("Users");

        var group = versionedApi.MapGroup("/v{version:apiVersion}/users")
            .WithTags("Users")
            .HasApiVersion(ApiVersions.V1)
            .WithOpenApi();

        group.MapGet("/", GetUsers)
            .WithName($"Users{nameof(GetUsers)}")
            .Produces<PagedResult<UserDto>>(StatusCodes.Status200OK);

        group.MapGet("/userInfo", GetUserInfo)
            .WithName($"Users_{nameof(GetUserInfo)}")
            .Produces<UserInfoDto>(StatusCodes.Status200OK);

        group.MapPost("/", CreateUser)
            .WithName($"Users_{nameof(CreateUser)}")
            .Produces<UserInfoDto>(StatusCodes.Status200OK);

        return app;
    }

    public static async Task<PagedResult<UserDto>> GetUsers(int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, SortDirection? sortDirection = null,
        IMediator mediator = default!, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetUsers(page, pageSize, searchTerm, sortBy, sortDirection), cancellationToken);

    public static async Task<UserInfoDto> GetUserInfo(IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserInfo(), cancellationToken);
        return result.GetValue();
    }

    public static async Task<UserInfoDto> CreateUser(CreateUserDto request, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateUser(request.Name, request.Email), cancellationToken);
        return result.GetValue();
    }
}

public sealed record CreateUserDto(string Name, string Email);
