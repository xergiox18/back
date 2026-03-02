using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Queries.GetByTemplate;

public sealed class GetQuestionsByTemplateValidator : AbstractValidator<GetQuestionsByTemplateQuery>
{
    public GetQuestionsByTemplateValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty().WithErrorCode("REQUIRED");
    }
}