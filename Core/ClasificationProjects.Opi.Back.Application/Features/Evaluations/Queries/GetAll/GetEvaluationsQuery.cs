using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Evaluations.Queries.GetAll;

public sealed record GetEvaluationsQuery() : IRequest<List<EvaluationListItemDto>>;

public sealed record EvaluationListItemDto(
    Guid Id,
    Guid? TemplateId,
    Guid RecommendedContractModelId,
    string ProjectName,
    string ClientName,
    DateTime EvaluatedAt,
    decimal Score,
    decimal Confidence
);