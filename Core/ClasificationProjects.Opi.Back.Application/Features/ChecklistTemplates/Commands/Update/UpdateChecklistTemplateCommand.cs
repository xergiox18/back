using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.Update;

public sealed record UpdateChecklistTemplateCommand(
    Guid Id,
    string Name,
    string Description
) : IRequest;

