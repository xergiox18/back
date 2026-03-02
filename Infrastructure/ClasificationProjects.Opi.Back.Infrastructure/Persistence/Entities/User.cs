namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities;

public class User
{
    public Guid Id { get; set; }

    public string TenantId { get; set; } = null!;
    public string ExternalId { get; set; } = null!;

    public Guid RoleId { get; set; }

    public string Email { get; set; } = null!;
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Role Role { get; set; } = null!;
}

