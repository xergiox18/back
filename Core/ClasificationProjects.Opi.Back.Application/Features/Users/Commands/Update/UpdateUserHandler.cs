using ClasificationProjects.Opi.Back.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Commands.Update;

public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _repo;
    private readonly IValidator<UpdateUserCommand> _validator;

    public UpdateUserHandler(
        IUserRepository repo,
        IValidator<UpdateUserCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var user = await _repo.GetByIdAsync(request.Id, ct);

        if (user is null)
            throw new KeyNotFoundException();

        user.Update(
            request.RoleId,
            request.Email,
            request.IsActive,
            DateTime.UtcNow
        );

        await _repo.UpdateAsync(user, ct);
    }
}