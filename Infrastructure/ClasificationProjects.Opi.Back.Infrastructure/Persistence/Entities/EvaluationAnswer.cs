namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities;

public class EvaluationAnswer
{
    public Guid EvaluationId { get; set; }
    public Guid QuestionId { get; set; }
    public bool Answer { get; set; }

    public Evaluation Evaluation { get; set; } = null!;
}
