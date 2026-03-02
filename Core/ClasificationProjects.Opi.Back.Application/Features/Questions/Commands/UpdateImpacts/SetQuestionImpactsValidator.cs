using FluentValidation;
using System.Linq;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.UpdateImpacts;

public sealed class SetQuestionImpactsValidator : AbstractValidator<SetQuestionImpactsCommand>
{
    public SetQuestionImpactsValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty().WithErrorCode("REQUIRED");

        RuleFor(x => x.Impacts)
            .NotNull().WithErrorCode("REQUIRED")
            .Must(x => x.Count == 4).WithErrorCode("EXACT_COUNT");

        RuleFor(x => x.Impacts)
            .Must(list => list.Select(i => i.ContractModelId).Distinct().Count() == list.Count)
            .WithErrorCode("DUPLICATE");

        RuleForEach(x => x.Impacts).ChildRules(i =>
        {
            i.RuleFor(z => z.ContractModelId)
                .NotEmpty().WithErrorCode("REQUIRED");
        });
    }
}