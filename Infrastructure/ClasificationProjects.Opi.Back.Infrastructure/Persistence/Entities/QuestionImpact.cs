namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities;

public class QuestionImpact
{
    public Guid QuestionId { get; set; }
    public Guid ContractModelId { get; set; }
    public int ImpactValue { get; set; }

    public Question Question { get; set; } = null!;
    public ContractModel ContractModel { get; set; } = null!;
}

