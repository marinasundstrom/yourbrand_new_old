using FluentValidation;

using Results2 = Microsoft.AspNetCore.Http.Results;

namespace YourBrand.YourService.API;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext ctx, EndpointFilterDelegate next)
    {
        var validator = ctx.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is not null)
        {
            var entity = ctx.Arguments
              .OfType<T>()
              .FirstOrDefault(a => a?.GetType() == typeof(T));
            if (entity is not null)
            {
                var validation = await validator.ValidateAsync(entity);
                if (validation.IsValid)
                {
                    return await next(ctx);
                }
                return Results2.ValidationProblem(validation.ToDictionary());
            }
            else
            {
                return Results2.Problem("Could not find type to validate");
            }
        }
        return await next(ctx);
    }
}