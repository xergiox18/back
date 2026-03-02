using ClasificationProjects.Opi.Back.Domain.Entities;
using ClasificationProjects.Opi.Back.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly ClasificationProjects.Opi.Back.Infrastructure.Persistence.DbContext _db;

    public UserRepository(ClasificationProjects.Opi.Back.Infrastructure.Persistence.DbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(User user, CancellationToken ct)
    {
        var entity = new Infrastructure.Persistence.Entities.User
        {
            Id = user.Id,
            TenantId = user.TenantId,
            ExternalId = user.ExternalId,
            RoleId = user.RoleId,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        _db.Users.Add(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(User user, CancellationToken ct)
    {
        var entity = await _db.Users.FirstOrDefaultAsync(x => x.Id == user.Id, ct);
        if (entity is null)
            return;

        entity.TenantId = user.TenantId;
        entity.ExternalId = user.ExternalId;
        entity.RoleId = user.RoleId;
        entity.Email = user.Email;
        entity.IsActive = user.IsActive;
        entity.CreatedAt = user.CreatedAt;
        entity.UpdatedAt = user.UpdatedAt;

        await _db.SaveChangesAsync(ct);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty)
            return null;

        var entity = await _db.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyList<User>> GetAsync(CancellationToken ct)
    {
        var entities = await _db.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .OrderBy(x => x.Email)
            .ToListAsync(ct);

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<User?> GetByTenantAndEmailAsync(string tenantId, string email, CancellationToken ct)
    {
        var t = NormalizeParam(tenantId);
        var e = NormalizeParam(email);

        if (string.IsNullOrWhiteSpace(t) || string.IsNullOrWhiteSpace(e))
            return null;

        var entity = await _db.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .FirstOrDefaultAsync(
                x =>
                    (x.TenantId ?? string.Empty).Trim().ToLower() == t &&
                    (x.Email ?? string.Empty).Trim().ToLower() == e,
                ct);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<User?> GetByTenantAndExternalIdAsync(string tenantId, string externalId, CancellationToken ct)
    {
        var t = NormalizeParam(tenantId);
        var xId = NormalizeParam(externalId);

        if (string.IsNullOrWhiteSpace(t) || string.IsNullOrWhiteSpace(xId))
            return null;

        var entity = await _db.Users
            .AsNoTracking()
            .Include(x => x.Role)
            .FirstOrDefaultAsync(
                x =>
                    (x.TenantId ?? string.Empty).Trim().ToLower() == t &&
                    (x.ExternalId ?? string.Empty).Trim().ToLower() == xId,
                ct);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task DeleteHardAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty)
            return;

        var entity = await _db.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
            return;

        _db.Users.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    private static string NormalizeParam(string? value)
        => (value ?? string.Empty).Trim().ToLower();

    private static User MapToDomain(Infrastructure.Persistence.Entities.User entity)
    {
        Role? role = null;

        if (entity.Role is not null)
            role = Role.Rehydrate(entity.Role.Id, entity.Role.Code, entity.Role.DisplayName);

        return User.Rehydrate(
            entity.Id,
            entity.TenantId,
            entity.ExternalId,
            entity.RoleId,
            entity.Email,
            entity.IsActive,
            entity.CreatedAt,
            entity.UpdatedAt,
            role
        );
    }
}