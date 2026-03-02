using Microsoft.EntityFrameworkCore;
using ClasificationProjects.Opi.Back.Domain.Aggregates;
using ClasificationProjects.Opi.Back.Domain.Entities;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Repositories;

public sealed class EvaluationRepository : IEvaluationRepository
{
    private readonly ClasificationProjects.Opi.Back.Infrastructure.Persistence.DbContext _db;

    public EvaluationRepository(ClasificationProjects.Opi.Back.Infrastructure.Persistence.DbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Evaluation aggregate, CancellationToken ct)
    {
        var evaluation = new Infrastructure.Persistence.Entities.Evaluation
        {
            Id = aggregate.Id,
            TemplateId = aggregate.TemplateId,
            RecommendedContractModelId = aggregate.RecommendedContractModelId,
            TemplateName = aggregate.TemplateName,
            TemplateDescription = aggregate.TemplateDescription,
            ProjectName = aggregate.ProjectName,
            ClientName = aggregate.ClientName,
            EvaluatedAt = aggregate.EvaluatedAt,
            Score = aggregate.Score,
            Confidence = aggregate.Confidence
        };

        _db.Evaluations.Add(evaluation);

        var answers = aggregate.Answers.Select(a =>
            new Infrastructure.Persistence.Entities.EvaluationAnswer
            {
                EvaluationId = a.EvaluationId,
                QuestionId = a.QuestionId,
                Answer = a.Answer
            });

        _db.EvaluationAnswers.AddRange(answers);

        await _db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<Evaluation>> GetAsync(CancellationToken ct)
    {
        var evaluations = await _db.Evaluations
            .AsNoTracking()
            .OrderByDescending(x => x.EvaluatedAt)
            .ToListAsync(ct);

        var evaluationIds = evaluations.Select(x => x.Id).ToList();

        var answers = await _db.EvaluationAnswers
            .AsNoTracking()
            .Where(x => evaluationIds.Contains(x.EvaluationId))
            .Select(x => new { x.EvaluationId, Answer = EvaluationAnswer.Create(x.EvaluationId, x.QuestionId, x.Answer) })
            .ToListAsync(ct);

        var answersByEvaluationId = answers
            .GroupBy(x => x.EvaluationId)
            .ToDictionary(g => g.Key, g => (IReadOnlyCollection<EvaluationAnswer>)g.Select(x => x.Answer).ToList());

        return evaluations
            .Select(e =>
            {
                answersByEvaluationId.TryGetValue(e.Id, out var list);
                list ??= Array.Empty<EvaluationAnswer>();

                return Evaluation.Rehydrate(
                    e.Id,
                    e.TemplateId,
                    e.RecommendedContractModelId,
                    e.TemplateName,
                    e.TemplateDescription,
                    e.ProjectName,
                    e.ClientName,
                    e.EvaluatedAt,
                    e.Score,
                    e.Confidence,
                    list
                );
            })
            .ToList();
    }

    public async Task<Evaluation?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty)
            return null;

        var evaluation = await _db.Evaluations
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, ct);

        if (evaluation is null)
            return null;

        var answers = await _db.EvaluationAnswers
            .AsNoTracking()
            .Where(x => x.EvaluationId == id)
            .Select(x => EvaluationAnswer.Create(x.EvaluationId, x.QuestionId, x.Answer))
            .ToListAsync(ct);

        return Evaluation.Rehydrate(
            evaluation.Id,
            evaluation.TemplateId,
            evaluation.RecommendedContractModelId,
            evaluation.TemplateName,
            evaluation.TemplateDescription,
            evaluation.ProjectName,
            evaluation.ClientName,
            evaluation.EvaluatedAt,
            evaluation.Score,
            evaluation.Confidence,
            answers
        );
    }

    public async Task<IReadOnlyList<Evaluation>> GetByTemplateAsync(Guid templateId, CancellationToken ct)
    {
        if (templateId == Guid.Empty)
            return Array.Empty<Evaluation>();

        var evaluations = await _db.Evaluations
            .AsNoTracking()
            .Where(x => x.TemplateId == templateId)
            .OrderByDescending(x => x.EvaluatedAt)
            .ToListAsync(ct);

        var evaluationIds = evaluations.Select(x => x.Id).ToList();

        var answers = await _db.EvaluationAnswers
            .AsNoTracking()
            .Where(x => evaluationIds.Contains(x.EvaluationId))
            .Select(x => new { x.EvaluationId, Answer = EvaluationAnswer.Create(x.EvaluationId, x.QuestionId, x.Answer) })
            .ToListAsync(ct);

        var answersByEvaluationId = answers
            .GroupBy(x => x.EvaluationId)
            .ToDictionary(g => g.Key, g => (IReadOnlyCollection<EvaluationAnswer>)g.Select(x => x.Answer).ToList());

        return evaluations
            .Select(e =>
            {
                answersByEvaluationId.TryGetValue(e.Id, out var list);
                list ??= Array.Empty<EvaluationAnswer>();

                return Evaluation.Rehydrate(
                    e.Id,
                    e.TemplateId,
                    e.RecommendedContractModelId,
                    e.TemplateName,
                    e.TemplateDescription,
                    e.ProjectName,
                    e.ClientName,
                    e.EvaluatedAt,
                    e.Score,
                    e.Confidence,
                    list
                );
            })
            .ToList();
    }

    public async Task DeleteHardAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty)
            return;

        var exists = await _db.Evaluations.AnyAsync(x => x.Id == id, ct);
        if (!exists)
            throw new KeyNotFoundException();

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        await _db.EvaluationAnswers
            .Where(x => x.EvaluationId == id)
            .ExecuteDeleteAsync(ct);

        await _db.Evaluations
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(ct);

        await tx.CommitAsync(ct);
    }
}
