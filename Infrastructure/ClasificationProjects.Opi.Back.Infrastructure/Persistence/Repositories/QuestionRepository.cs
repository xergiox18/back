using Microsoft.EntityFrameworkCore;
using ClasificationProjects.Opi.Back.Domain.Entities;
using ClasificationProjects.Opi.Back.Domain.Repositories;
using ClasificationProjects.Opi.Back.Domain.ValueObjects;

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Repositories;

public sealed class QuestionRepository : IQuestionRepository
{
    private readonly ClasificationProjects.Opi.Back.Infrastructure.Persistence.DbContext _db;

    public QuestionRepository(ClasificationProjects.Opi.Back.Infrastructure.Persistence.DbContext db)
    {
        _db = db;
    }

    public async Task AddManyAsync(Guid templateId, IReadOnlyList<Question> questions, CancellationToken ct)
    {
        var questionEntities = new List<ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities.Question>(questions.Count);
        var impactEntities = new List<ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities.QuestionImpact>(questions.Count * 4);

        foreach (var q in questions)
        {
            questionEntities.Add(new ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities.Question
            {
                Id = q.Id,
                TemplateId = q.TemplateId,
                Category = q.Category,
                Text = q.Text,
                Order = q.Order,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt
            });

            foreach (var impact in q.Impacts)
            {
                impactEntities.Add(new ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities.QuestionImpact
                {
                    QuestionId = q.Id,
                    ContractModelId = impact.ContractModelId,
                    ImpactValue = impact.ImpactValue
                });
            }
        }

        _db.Questions.AddRange(questionEntities);
        _db.QuestionImpacts.AddRange(impactEntities);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<Question?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty)
            return null;

        var row = await _db.Questions
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, ct);

        if (row is null)
            return null;

        var impacts = await _db.QuestionImpacts
            .AsNoTracking()
            .Where(x => x.QuestionId == id)
            .Select(x => QuestionImpact.Create(x.ContractModelId, x.ImpactValue))
            .ToListAsync(ct);

        return Question.Rehydrate(
            row.Id,
            row.TemplateId,
            row.Category,
            row.Text,
            row.Order,
            row.CreatedAt,
            row.UpdatedAt,
            impacts
        );
    }

    public async Task UpdateAsync(Question entity, CancellationToken ct)
    {
        var row = await _db.Questions.SingleOrDefaultAsync(x => x.Id == entity.Id, ct);

        if (row is null)
            throw new KeyNotFoundException();

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        row.Category = entity.Category;
        row.Text = entity.Text;
        row.Order = entity.Order;
        row.UpdatedAt = entity.UpdatedAt;

        await _db.QuestionImpacts
            .Where(x => x.QuestionId == entity.Id)
            .ExecuteDeleteAsync(ct);

        var newImpacts = entity.Impacts
            .Select(i => new ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities.QuestionImpact
            {
                QuestionId = entity.Id,
                ContractModelId = i.ContractModelId,
                ImpactValue = i.ImpactValue
            })
            .ToList();

        if (newImpacts.Count > 0)
            _db.QuestionImpacts.AddRange(newImpacts);

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

    public async Task DeleteHardAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty)
            return;

        var exists = await _db.Questions.AnyAsync(x => x.Id == id, ct);
        if (!exists)
            throw new KeyNotFoundException();

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        await _db.QuestionImpacts
            .Where(x => x.QuestionId == id)
            .ExecuteDeleteAsync(ct);

        await _db.Questions
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(ct);

        await tx.CommitAsync(ct);
    }
}
