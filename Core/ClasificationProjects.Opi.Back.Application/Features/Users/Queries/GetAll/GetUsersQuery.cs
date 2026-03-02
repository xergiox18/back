using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Queries.GetAll;

public sealed record GetUsersQuery() : IRequest<List<UserListItemDto>>;

public sealed record UserListItemDto(
    Guid Id,
    string TenantId,
    string ExternalId,
    Guid RoleId,
    string RoleCode,
    string RoleDisplayName,
    string Email,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);