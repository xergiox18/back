using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.Update;

public sealed class UpdateChecklistTemplateValidator : AbstractValidator<UpdateChecklistTemplateCommand>
{
    public UpdateChecklistTemplateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode("REQUIRED");

        RuleFor(x => x.Name)
            .NotEmpty().WithErrorCode("REQUIRED")
            .MaximumLength(200).WithErrorCode("MAX_LENGTH");

        RuleFor(x => x.Description)
            .NotEmpty().WithErrorCode("REQUIRED")
            .MaximumLength(2000).WithErrorCode("MAX_LENGTH");
    }
}