namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities;

public class ChecklistTemplate
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

