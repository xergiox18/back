using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.Update;

public sealed class UpdateQuestionValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode("REQUIRED");

        RuleFor(x => x.Category)
            .NotEmpty().WithErrorCode("REQUIRED")
            .MaximumLength(100).WithErrorCode("MAX_LENGTH");

        RuleFor(x => x.Text)
            .NotEmpty().WithErrorCode("REQUIRED")
            .MaximumLength(2000).WithErrorCode("MAX_LENGTH");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(1).WithErrorCode("MIN_VALUE");
    }
}