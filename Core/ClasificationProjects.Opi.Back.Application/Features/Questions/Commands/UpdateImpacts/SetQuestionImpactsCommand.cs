using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.UpdateImpacts;

public sealed record SetQuestionImpactsCommand(
    Guid QuestionId,
    List<SetQuestionImpactDto> Impacts
) : IRequest;

public sealed record SetQuestionImpactDto(
    Guid ContractModelId,
    int ImpactValue
);

