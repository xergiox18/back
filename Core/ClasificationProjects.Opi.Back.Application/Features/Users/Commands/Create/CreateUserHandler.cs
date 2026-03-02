using ClasificationProjects.Opi.Back.Domain.Entities;
using ClasificationProjects.Opi.Back.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Commands.Create;

public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _repo;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserHandler(
        IUserRepository repo,
        IValidator<CreateUserCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var now = DateTime.UtcNow;

        var aggregate = User.Create(
            request.TenantId,
            "PENDING",
            request.RoleId,
            request.Email,
            now);

        await _repo.AddAsync(aggregate, ct);

        return aggregate.Id;
    }
}