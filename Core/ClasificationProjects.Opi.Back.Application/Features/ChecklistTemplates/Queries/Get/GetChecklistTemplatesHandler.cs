using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Queries.Get;

public sealed class GetChecklistTemplatesHandler : IRequestHandler<GetChecklistTemplatesQuery, List<ChecklistTemplateDto>>
{
    private readonly IChecklistTemplateRepository _repo;

    public GetChecklistTemplatesHandler(IChecklistTemplateRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ChecklistTemplateDto>> Handle(GetChecklistTemplatesQuery request, CancellationToken ct)
    {
        var all = await _repo.GetAsync(ct);

        var filtered = request.Status switch
        {
            ChecklistTemplateStatusFilter.Active => all.Where(x => x.IsActive),
            ChecklistTemplateStatusFilter.Inactive => all.Where(x => !x.IsActive),
            _ => all
        };

        return filtered
            .OrderBy(x => x.Name)
            .Select(x => new ChecklistTemplateDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedAt,
                x.UpdatedAt,
                x.Questions
                    .OrderBy(q => q.Order)
                    .Select(q => new ChecklistTemplateQuestionDto(
                        q.Id,
                        q.Text,
                        q.Category,
                        q.Order,
                        q.Impacts
                            .Select(i => new QuestionImpactDto(i.ContractModelId, i.ImpactValue))
                            .ToList()
                    ))
                    .ToList()
            ))
            .ToList();
    }
}