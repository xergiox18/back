using FluentValidation;
using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.Update;

public sealed class UpdateQuestionHandler : IRequestHandler<UpdateQuestionCommand>
{
    private readonly IQuestionRepository _questionRepo;
    private readonly IChecklistTemplateRepository _templateRepo;
    private readonly IValidator<UpdateQuestionCommand> _validator;

    public UpdateQuestionHandler(
        IQuestionRepository questionRepo,
        IChecklistTemplateRepository templateRepo,
        IValidator<UpdateQuestionCommand> validator)
    {
        _questionRepo = questionRepo;
        _templateRepo = templateRepo;
        _validator = validator;
    }

    public async Task Handle(UpdateQuestionCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var question = await _questionRepo.GetByIdAsync(request.Id, ct);

        if (question is null)
            throw new KeyNotFoundException();

        var template = await _templateRepo.GetByIdAsync(question.TemplateId, ct);

        if (template is null)
            throw new KeyNotFoundException();

        var orderAlreadyExists = template.Questions.Any(x => x.Id != question.Id && x.Order == request.Order);

        if (orderAlreadyExists)
            throw new InvalidOperationException();

        question.Update(request.Category, request.Text, request.Order, DateTime.UtcNow);

        await _questionRepo.UpdateAsync(question, ct);
    }
}