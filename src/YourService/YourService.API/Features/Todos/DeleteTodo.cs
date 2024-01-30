using MediatR;
using YourBrand.YourService.API.Repositories;
using FluentValidation;

namespace YourBrand.YourService.API.Features.Todos;

public sealed record DeleteTodo(string Id) : IRequest<Result>
{
    public sealed class DeleteTodoValidator : AbstractValidator<DeleteTodo>
    {
        public DeleteTodoValidator()
        {
            RuleFor(p => p.Id).MaximumLength(120).NotEmpty();
        }
    }

    public sealed class Handler(ITodoRepository todoRepository) : IRequestHandler<DeleteTodo, Result>
    {
        public async Task<Result> Handle(DeleteTodo request, CancellationToken cancellationToken)
        {
            var todosRemoved = await todoRepository.RemoveByIdAsync(request.Id);

            if (todosRemoved == 0)
            {
                return Errors.Todos.TodoNotFound;
            }

            return Results.Success;
        }
    }
}