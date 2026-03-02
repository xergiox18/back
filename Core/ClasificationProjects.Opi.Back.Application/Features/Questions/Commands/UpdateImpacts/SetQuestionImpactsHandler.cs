using FluentValidation;
using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;
using ClasificationProjects.Opi.Back.Domain.ValueObjects;

namespace ClasificationProjects.Opi.Back.Application.Features.Questions.Commands.UpdateImpacts;

public sealed class SetQuestionImpactsHandler : IRequestHandler<SetQuestionImpactsCommand>
{
    private readonly IQuestionRepository _questionRepo;
    private readonly IValidator<SetQuestionImpactsCommand> _validator;

    public SetQuestionImpactsHandler(
        IQuestionRepository questionRepo,
        IValidator<SetQuestionImpactsCommand> validator)
    {
        _questionRepo = questionRepo;
        _validator = validator;
    }

    public async Task Handle(SetQuestionImpactsCommand request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var question = await _questionRepo.GetByIdAsync(request.QuestionId, ct);

        if (question is null)
            throw new KeyNotFoundException();

        var impacts = request.Impacts
            .Select(i => QuestionImpact.Create(i.ContractModelId, i.ImpactValue))
            .ToList();

        question.SetImpacts(impacts, DateTime.UtcNow);

        await _questionRepo.UpdateAsync(question, ct);
    }
}