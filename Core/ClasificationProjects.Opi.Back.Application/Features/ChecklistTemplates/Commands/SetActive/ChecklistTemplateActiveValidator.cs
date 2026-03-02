using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.SetActive;

public sealed class ChecklistTemplateActiveValidator : AbstractValidator<ChecklistTemplateActiveCommand>
{
    public ChecklistTemplateActiveValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode("REQUIRED");
    }
}