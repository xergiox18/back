namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities;

public class Question
{
    public Guid Id { get; set; }
    public Guid TemplateId { get; set; }
    public string Category { get; set; } = null!;
    public string Text { get; set; } = null!;
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ChecklistTemplate Template { get; set; } = null!;
}

