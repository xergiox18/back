using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Evaluations.Queries.GetById;

public sealed record GetEvaluationByIdQuery(Guid Id) : IRequest<EvaluationDetailsDto>;

public sealed record EvaluationDetailsDto(
    Guid Id,
    Guid? TemplateId,
    Guid RecommendedContractModelId,
    string ProjectName,
    string ClientName,
    DateTime EvaluatedAt,
    decimal Score,
    decimal Confidence,
    List<EvaluationAnswerDto> Answers
);

public sealed record EvaluationAnswerDto(
    Guid QuestionId,
    bool Answer
);