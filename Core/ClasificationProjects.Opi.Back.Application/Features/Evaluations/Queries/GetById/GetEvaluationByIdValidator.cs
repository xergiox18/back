using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.Evaluations.Queries.GetById;

public sealed class GetEvaluationByIdValidator : AbstractValidator<GetEvaluationByIdQuery>
{
    public GetEvaluationByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode("REQUIRED");
    }
}