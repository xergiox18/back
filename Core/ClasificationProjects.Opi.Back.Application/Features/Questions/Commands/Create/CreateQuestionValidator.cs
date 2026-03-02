using FluentValidation;
using System.Linq;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.Create;

public sealed class CreateQuestionsValidator : AbstractValidator<CreateQuestionsCommand>
{
    public CreateQuestionsValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty().WithErrorCode("REQUIRED");

        RuleFor(x => x.Questions)
            .NotNull().WithErrorCode("REQUIRED")
            .Must(x => x.Count > 0).WithErrorCode("MIN_ITEMS");

        RuleForEach(x => x.Questions).ChildRules(q =>
        {
            q.RuleFor(x => x.Category)
                .NotEmpty().WithErrorCode("REQUIRED")
                .MaximumLength(100).WithErrorCode("MAX_LENGTH");

            q.RuleFor(x => x.Text)
                .NotEmpty().WithErrorCode("REQUIRED")
                .MaximumLength(2000).WithErrorCode("MAX_LENGTH");

            q.RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(1).WithErrorCode("MIN_VALUE");

            q.RuleFor(x => x.Impacts)
                .NotNull().WithErrorCode("REQUIRED")
                .Must(x => x.Count == 4).WithErrorCode("EXACT_COUNT");

            q.RuleFor(x => x.Impacts)
                .Must(list => list.Select(i => i.ContractModelId).Distinct().Count() == list.Count)
                .WithErrorCode("DUPLICATE");

            q.RuleForEach(x => x.Impacts).ChildRules(i =>
            {
                i.RuleFor(z => z.ContractModelId)
                    .NotEmpty().WithErrorCode("REQUIRED");
            });
        });
    }
}