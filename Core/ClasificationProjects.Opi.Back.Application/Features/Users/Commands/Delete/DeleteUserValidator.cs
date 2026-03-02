using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Commands.Delete;

public sealed class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode("REQUIRED");
    }
}