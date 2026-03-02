using ClasificationProjects.Opi.Back.Domain.Repositories;
using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Queries.GetAll;

public sealed class GetUsersHandler : IRequestHandler<GetUsersQuery, List<UserListItemDto>>
{
    private readonly IUserRepository _repo;

    public GetUsersHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<UserListItemDto>> Handle(GetUsersQuery request, CancellationToken ct)
    {
        var users = await _repo.GetAsync(ct);

        return users
            .OrderBy(x => x.Email)
            .ThenBy(x => x.Id)
            .Select(x => new UserListItemDto(
                x.Id,
                x.TenantId,
                x.ExternalId,
                x.RoleId,
                x.Role.Code,
                x.Role.DisplayName,
                x.Email,
                x.IsActive,
                x.CreatedAt,
                x.UpdatedAt
            ))
            .ToList();
    }
}