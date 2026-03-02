using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.Create;

public sealed record CreateQuestionsCommand(
    Guid TemplateId,
    List<CreateQuestionItemDto> Questions
) : IRequest<List<Guid>>;

public sealed record CreateQuestionItemDto(
    string Category,
    string Text,
    int Order,
    List<CreateQuestionImpactDto> Impacts
);

public sealed record CreateQuestionImpactDto(
    Guid ContractModelId,
    int ImpactValue
);

