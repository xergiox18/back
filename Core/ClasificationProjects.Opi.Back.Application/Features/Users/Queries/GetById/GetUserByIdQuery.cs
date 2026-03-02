using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Queries.GetById;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<UserDetailsDto>;

public sealed record UserDetailsDto(
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