using FluentValidation;

using MediatR;

using YourBrand.YourService.API.Domain.Entities;
using YourBrand.YourService.API.Repositories;

namespace YourBrand.YourService.API.Features.Todos;

public sealed record CreateTodo(string Text) : IRequest<Result<TodoDto>>
{
    public sealed class CreateTodoValidator : AbstractValidator<CreateTodo>
    {
        public CreateTodoValidator()
        {
            RuleFor(p => p.Text).MaximumLength(120).NotEmpty();
        }
    }

    public sealed class Handler(ITodoRepository todoRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateTodo, Result<TodoDto>>
    {
        public async Task<Result<TodoDto>> Handle(CreateTodo request, CancellationToken cancellationToken)
        {
            var todo = Todo.Create(request.Text);

            todoRepository.Add(todo);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return todo.ToDto();
        }
    }
}