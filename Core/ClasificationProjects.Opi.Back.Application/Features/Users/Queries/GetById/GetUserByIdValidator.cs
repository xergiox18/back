using FluentValidation;

namespace ClasificationProjects.Opi.Back.Application.Features.Users.Queries.GetById;

public sealed class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithErrorCode("REQUIRED");
    }
}