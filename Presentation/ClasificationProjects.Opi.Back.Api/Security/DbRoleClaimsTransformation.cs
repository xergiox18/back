using System.Security.Claims;
using ClasificationProjects.Opi.Back.Domain.Repositories;
using Microsoft.AspNetCore.Authentication;

namespace ClasificationProjects.Opi.Back.Api.Security;

public sealed class DbRoleClaimsTransformation : IClaimsTransformation
{
    private readonly IUserRepository _users;

    public DbRoleClaimsTransformation(IUserRepository users)
    {
        _users = users;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identities.FirstOrDefault(i => i is not null && i.IsAuthenticated) as ClaimsIdentity;
        if (identity is null)
            return principal;

        var tenantId = GetClaimValue(principal, "tid");
        if (string.IsNullOrWhiteSpace(tenantId))
            return principal;

        var externalId = GetClaimValue(principal, "oid");

        var email =
            GetClaimValue(principal, "preferred_username") ??
            GetClaimValue(principal, "upn") ??
            GetClaimValue(principal, "email");

        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(externalId))
            return principal;

        var user = !string.IsNullOrWhiteSpace(externalId)
            ? await _users.GetByTenantAndExternalIdAsync(tenantId, externalId, CancellationToken.None)
            : null;

        if (user is null && !string.IsNullOrWhiteSpace(email))
            user = await _users.GetByTenantAndEmailAsync(tenantId, email, CancellationToken.None);

        if (user is null)
            return principal;

        if (!user.IsActive)
            return principal;

        if (!string.IsNullOrWhiteSpace(externalId) &&
            string.Equals(user.ExternalId, "PENDING", StringComparison.OrdinalIgnoreCase))
        {
            user.SetExternalId(externalId, DateTime.UtcNow);
            await _users.UpdateAsync(user, CancellationToken.None);

            var refreshed = await _users.GetByTenantAndExternalIdAsync(tenantId, externalId, CancellationToken.None);
            if (refreshed is not null)
                user = refreshed;
        }

        var roleCode = user.Role?.Code;
        if (string.IsNullOrWhiteSpace(roleCode))
            return principal;

        EnsureClaim(identity, ClaimTypes.Role, roleCode);

        EnsureClaim(identity, "user_id", user.Id.ToString());
        EnsureClaim(identity, "user_email", user.Email);

        var name = GetClaimValue(principal, "name");
        if (string.IsNullOrWhiteSpace(name))
            name = user.Email;

        EnsureClaim(identity, "user_name", name);

        return principal;
    }

    private static void EnsureClaim(ClaimsIdentity identity, string type, string value)
    {
        var exists = identity.Claims.Any(c =>
            string.Equals(c.Type, type, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(c.Value, value, StringComparison.OrdinalIgnoreCase));

        if (!exists)
            identity.AddClaim(new Claim(type, value));
    }

    private static string? GetClaimValue(ClaimsPrincipal principal, string type)
        => principal.Claims.FirstOrDefault(c => string.Equals(c.Type, type, StringComparison.OrdinalIgnoreCase))?.Value;
}