using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.Create;

public sealed class CreateChecklistTemplateValidator : AbstractValidator<CreateChecklistTemplateCommand>
{
    public CreateChecklistTemplateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithErrorCode("REQUIRED")
            .MaximumLength(200).WithErrorCode("MAX_LENGTH");

        RuleFor(x => x.Description)
            .NotEmpty().WithErrorCode("REQUIRED")
            .MaximumLength(2000).WithErrorCode("MAX_LENGTH");
    }
}