using ClasificationProjects.Opi.Back.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Commands.Delete;

public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _repo;
    private readonly IValidator<DeleteUserCommand> _validator;

    public DeleteUserHandler(
        IUserRepository repo,
        IValidator<DeleteUserCommand> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        await _repo.DeleteHardAsync(request.Id, ct);
    }
}