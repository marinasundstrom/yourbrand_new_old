using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.YourService.API;
using YourBrand.YourService.API.Repositories;
using YourBrand.YourService.API.Common;
using FluentValidation;
using YourBrand.YourService.API.Domain.Entities;

namespace YourBrand.YourService.API.Features.Todos;

public record CreateTodo(string Text) : IRequest<Result<TodoDto>>
{
    public class CreateTodoValidator : AbstractValidator<CreateTodo>
    {
        public CreateTodoValidator()
        {
            RuleFor(p => p.Text).MaximumLength(120).NotEmpty();
        }
    }

    public class Handler(ITodoRepository todoRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateTodo, Result<TodoDto>>
    {
        public async Task<Result<TodoDto>> Handle(CreateTodo request, CancellationToken cancellationToken)
        {
            var todo = Todo.Create(request.Text);

            todoRepository.Add(todo);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(todo.ToDto());
        }
    }
}