namespace ClasificationProjects.Opi.Back.Application.Features.ChecklistTemplates.Queries.Get;

public sealed record ChecklistTemplateDto(
    Guid Id,
    string Name,
    string Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<ChecklistTemplateQuestionDto> Questions
);

public sealed record ChecklistTemplateQuestionDto(
    Guid Id,
    string Text,
    string Category,
    int Order,
    IReadOnlyList<QuestionImpactDto> Impacts
);

public sealed record QuestionImpactDto(
    Guid ContractModelId,
    int ImpactValue
);
