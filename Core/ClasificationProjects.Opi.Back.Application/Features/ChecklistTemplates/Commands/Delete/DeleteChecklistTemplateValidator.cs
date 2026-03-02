using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.Delete;

public sealed class DeleteChecklistTemplateValidator : AbstractValidator<DeleteChecklistTemplateCommand>
{
    public DeleteChecklistTemplateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode("REQUIRED");
    }
}