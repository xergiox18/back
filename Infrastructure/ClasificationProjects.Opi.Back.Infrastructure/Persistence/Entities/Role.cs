namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
}

