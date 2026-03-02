using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Commands.Update;

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode("REQUIRED");

        RuleFor(x => x.RoleId)
            .NotEmpty().WithErrorCode("REQUIRED");

        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("REQUIRED")
            .MaximumLength(200).WithErrorCode("MAX_LENGTH")
            .EmailAddress().WithErrorCode("INVALID");
    }
}