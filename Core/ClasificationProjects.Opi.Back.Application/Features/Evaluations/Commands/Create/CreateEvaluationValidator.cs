using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.Evaluations.Commands.Create;

public sealed class CreateEvaluationValidator : AbstractValidator<CreateEvaluationCommand>
{
    public CreateEvaluationValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty().WithErrorCode("REQUIRED");

        RuleFor(x => x.ProjectName)
            .NotEmpty().WithErrorCode("REQUIRED")
            .MaximumLength(200).WithErrorCode("MAX_LENGTH");

        RuleFor(x => x.ClientName)
            .NotEmpty().WithErrorCode("REQUIRED")
            .MaximumLength(200).WithErrorCode("MAX_LENGTH");

        RuleFor(x => x.Answers)
            .NotNull().WithErrorCode("REQUIRED")
            .Must(x => x.Count > 0).WithErrorCode("MIN_ITEMS");

        RuleForEach(x => x.Answers).ChildRules(a =>
        {
            a.RuleFor(z => z.QuestionId)
                .NotEmpty().WithErrorCode("REQUIRED");
        });
    }
}