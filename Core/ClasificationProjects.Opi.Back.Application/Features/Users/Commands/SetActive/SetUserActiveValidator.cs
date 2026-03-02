using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Commands.SetActive;

public sealed class SetUserActiveValidator : AbstractValidator<SetUserActiveCommand>
{
    public SetUserActiveValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode("REQUIRED");
    }
}