using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Queries.GetByTemplate;

public sealed record GetQuestionsByTemplateQuery(Guid TemplateId) : IRequest<List<QuestionDto>>;

public sealed record QuestionDto(
    Guid Id,
    string Category,
    string Text,
    int Order,
    List<QuestionImpactDto> Impacts
);

public sealed record QuestionImpactDto(
    Guid ContractModelId,
    int ImpactValue
);

