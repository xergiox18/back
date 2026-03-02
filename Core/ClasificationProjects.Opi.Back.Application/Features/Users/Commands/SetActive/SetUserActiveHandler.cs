using ClasificationProjects.Opi.Back.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Commands.SetActive;

public sealed class SetUserActiveHandler : IRequestHandler<SetUserActiveCommand>
{
    private readonly IUserRepository _repo;
    private readonly IValidator<SetUserActiveCommand> _validator;

    public SetUserActiveHandler(
        IUserRepository repo,
        IValidator<SetUserActiveCommand> _validator)
    {
        _repo = repo;
        this._validator = _validator;
    }

    public async Task Handle(SetUserActiveCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var user = await _repo.GetByIdAsync(request.Id, ct);

        if (user is null)
            throw new KeyNotFoundException();

        user.Update(user.RoleId, user.Email, request.IsActive, DateTime.UtcNow);

        await _repo.UpdateAsync(user, ct);
    }
}