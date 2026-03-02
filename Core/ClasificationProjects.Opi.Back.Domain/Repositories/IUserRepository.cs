using ClasificationProjects.Opi.Back.Domain.Entities;

namespace ClasificationProjects.Opi.Back.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken ct);
    Task UpdateAsync(User user, CancellationToken ct);

    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<User>> GetAsync(CancellationToken ct);

    Task<User?> GetByTenantAndEmailAsync(string tenantId, string email, CancellationToken ct);
    Task<User?> GetByTenantAndExternalIdAsync(string tenantId, string externalId, CancellationToken ct);

    Task DeleteHardAsync(Guid id, CancellationToken ct);
}