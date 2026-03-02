using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Application.Features.Evaluations.Queries.GetAll;

public sealed class GetEvaluationsHandler : IRequestHandler<GetEvaluationsQuery, List<EvaluationListItemDto>>
{
    private readonly IEvaluationRepository _evaluationRepo;

    public GetEvaluationsHandler(IEvaluationRepository evaluationRepo)
    {
        _evaluationRepo = evaluationRepo;
    }

    public async Task<List<EvaluationListItemDto>> Handle(GetEvaluationsQuery request, CancellationToken ct)
    {
        var evals = await _evaluationRepo.GetAsync(ct);

        return evals
            .OrderByDescending(x => x.EvaluatedAt)
            .ThenBy(x => x.Id)
            .Select(e =>
            {
                var templateId = e.TemplateId ?? throw new InvalidOperationException();

                return new EvaluationListItemDto(
                    e.Id,
                    templateId,
                    e.RecommendedContractModelId,
                    e.ProjectName,
                    e.ClientName,
                    e.EvaluatedAt,
                    e.Score,
                    e.Confidence
                );
            })
            .ToList();
    }
}