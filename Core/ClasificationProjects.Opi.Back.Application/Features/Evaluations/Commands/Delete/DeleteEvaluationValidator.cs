using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.Evaluations.Commands.Delete;

public sealed class DeleteEvaluationValidator : AbstractValidator<DeleteEvaluationCommand>
{
    public DeleteEvaluationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode("REQUIRED");
    }
}