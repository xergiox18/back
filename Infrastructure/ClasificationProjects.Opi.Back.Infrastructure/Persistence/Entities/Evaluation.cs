namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities;

public class Evaluation
{
    public Guid Id { get; set; }

    public Guid? TemplateId { get; set; }
    public Guid RecommendedContractModelId { get; set; }

    public string ProjectName { get; set; } = null!;
    public string ClientName { get; set; } = null!;

    public DateTime EvaluatedAt { get; set; }

    public decimal Score { get; set; }
    public decimal Confidence { get; set; }

    public string TemplateName { get; set; } = null!;
    public string TemplateDescription { get; set; } = null!;

    public ChecklistTemplate? Template { get; set; }
    public ContractModel RecommendedContractModel { get; set; } = null!;
}
