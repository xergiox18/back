using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Commands.Delete;

public sealed record DeleteChecklistTemplateCommand(Guid Id) : IRequest;
