using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.SetActive;

public sealed record ChecklistTemplateActiveCommand(
    Guid Id,
    bool IsActive
) : IRequest;

