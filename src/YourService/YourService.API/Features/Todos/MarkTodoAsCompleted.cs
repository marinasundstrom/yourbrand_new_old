using MediatR;
using YourBrand.YourService.API.Repositories;
using FluentValidation;

namespace YourBrand.YourService.API.Features.Todos;

public sealed record MarkTodoAsCompleted(Guid Id) : IRequest<Result>
{
    public sealed class MarkTodoAsCompletedValidator : AbstractValidator<MarkTodoAsCompleted>
    {
        public MarkTodoAsCompletedValidator()
        {
            //RuleFor(p => p.Id).MaximumLength(120).NotEmpty();
        }
    }

    public sealed class Handler(ITodoRepository todoRepository, IUnitOfWork unitOfWork) : IRequestHandler<MarkTodoAsCompleted, Result>
    {
        public async Task<Result> Handle(MarkTodoAsCompleted request, CancellationToken cancellationToken)
        {
            var todo = await todoRepository.FindByIdAsync(request.Id, cancellationToken);

            if (todo is null)
            {
                return Errors.Todos.TodoNotFound;
            }

            todo.IsCompleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Results.Success;
        }
    }
}