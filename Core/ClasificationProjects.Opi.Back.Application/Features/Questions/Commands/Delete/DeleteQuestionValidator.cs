using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.Delete;

public sealed class DeleteQuestionValidator : AbstractValidator<DeleteQuestionCommand>
{
    public DeleteQuestionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode("REQUIRED");
    }
}