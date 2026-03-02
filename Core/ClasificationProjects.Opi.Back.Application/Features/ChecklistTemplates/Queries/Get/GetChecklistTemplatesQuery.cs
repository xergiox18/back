using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Queries.Get;

public sealed record GetChecklistTemplatesQuery(
    ChecklistTemplateStatusFilter Status = ChecklistTemplateStatusFilter.All
) : IRequest<List<ChecklistTemplateDto>>;

