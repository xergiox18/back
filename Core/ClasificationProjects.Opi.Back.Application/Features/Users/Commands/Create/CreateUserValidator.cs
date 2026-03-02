using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Commands.Create;

public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithErrorCode("REQUIRED")
            .MaximumLength(50).WithErrorCode("MAX_LENGTH");

        RuleFor(x => x.RoleId)
            .NotEmpty().WithErrorCode("REQUIRED");

        RuleFor(x => x.Email)
            .NotEmpty().WithErrorCode("REQUIRED")
            .MaximumLength(200).WithErrorCode("MAX_LENGTH")
            .EmailAddress().WithErrorCode("INVALID");
    }
}