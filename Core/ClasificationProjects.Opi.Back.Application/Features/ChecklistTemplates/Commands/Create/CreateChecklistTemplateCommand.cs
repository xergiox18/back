using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.Create;

public sealed record CreateChecklistTemplateCommand(
    string Name,
    string Description,
    bool IsActive
) : IRequest<Guid>;

