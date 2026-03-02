using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Evaluations.Commands.Create;

public sealed record CreateEvaluationCommand(
    Guid TemplateId,
    string ProjectName,
    string ClientName,
    IReadOnlyList<CreateEvaluationAnswerDto> Answers
) : IRequest<CreateEvaluationResultDto>;

public sealed record CreateEvaluationAnswerDto(
    Guid QuestionId,
    bool Answer
);

public sealed record CreateEvaluationResultDto(
    Guid EvaluationId,
    Guid RecommendedContractModelId,
    decimal Score,
    decimal Confidence
);
