using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Queries.GetAll;

public sealed class GetUsersValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersValidator()
    {
    }
}