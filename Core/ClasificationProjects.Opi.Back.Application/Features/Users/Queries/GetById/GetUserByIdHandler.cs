using ClasificationProjects.Opi.Back.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Queries.GetById;

public sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDetailsDto>
{
    private readonly IUserRepository _repo;
    private readonly IValidator<GetUserByIdQuery> _validator;

    public GetUserByIdHandler(IUserRepository repo, IValidator<GetUserByIdQuery> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task<UserDetailsDto> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var user = await _repo.GetByIdAsync(request.Id, ct);

        if (user is null)
            throw new KeyNotFoundException();

        return new UserDetailsDto(
            user.Id,
            user.TenantId,
            user.ExternalId,
            user.RoleId,
            user.Role.Code,
            user.Role.DisplayName,
            user.Email,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt
        );
    }
}