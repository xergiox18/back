using Microsoft.EntityFrameworkCore;
using ClasificationProjects.Opi.Back.Domain.Aggregates;
using ClasificationProjects.Opi.Back.Domain.Entities;
using ClasificationProjects.Opi.Back.Domain.Repositories;
using ClasificationProjects.Opi.Back.Domain.ValueObjects;

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Repositories;

public sealed class ChecklistTemplateRepository : IChecklistTemplateRepository
{
    private readonly ClasificationProjects.Opi.Back.Infrastructure.Persistence.DbContext _db;

    public ChecklistTemplateRepository(ClasificationProjects.Opi.Back.Infrastructure.Persistence.DbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(ChecklistTemplate aggregate, CancellationToken ct)
    {
        var entity = new Infrastructure.Persistence.Entities.ChecklistTemplate
        {
            Id = aggregate.Id,
            Name = aggregate.Name,
            Description = aggregate.Description,
            IsActive = aggregate.IsActive,
            CreatedAt = aggregate.CreatedAt,
            UpdatedAt = aggregate.UpdatedAt
        };

        _db.ChecklistTemplates.Add(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(ChecklistTemplate aggregate, CancellationToken ct)
    {
        var entity = await _db.ChecklistTemplates
            .SingleOrDefaultAsync(x => x.Id == aggregate.Id, ct);

        if (entity is null)
            throw new KeyNotFoundException();

        entity.Name = aggregate.Name;
        entity.Description = aggregate.Description;
        entity.IsActive = aggregate.IsActive;
        entity.UpdatedAt = aggregate.UpdatedAt;

        await _db.SaveChangesAsync(ct);
    }

    public async Task<ChecklistTemplate?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty)
            return null;

        var template = await _db.ChecklistTemplates
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, ct);

        if (template is null)
            return null;

        var questionRows = await _db.Questions
            .AsNoTracking()
            .Where(x => x.TemplateId == id)
            .OrderBy(x => x.Order)
            .Select(x => new
            {
                x.Id,
                x.TemplateId,
                x.Category,
                x.Text,
                x.Order,
                x.CreatedAt,
                x.UpdatedAt
            })
            .ToListAsync(ct);

        var questionIds = questionRows.Select(x => x.Id).ToList();

        var impactRows = await _db.QuestionImpacts
            .AsNoTracking()
            .Where(x => questionIds.Contains(x.QuestionId))
            .Select(x => new
            {
                x.QuestionId,
                x.ContractModelId,
                x.ImpactValue
            })
            .ToListAsync(ct);

        var impactsByQuestionId = impactRows
            .GroupBy(x => x.QuestionId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyCollection<QuestionImpact>)g
                    .Select(i => QuestionImpact.Create(i.ContractModelId, i.ImpactValue))
                    .ToList()
            );

        var questions = questionRows
            .Select(q =>
            {
                impactsByQuestionId.TryGetValue(q.Id, out var impacts);
                impacts ??= Array.Empty<QuestionImpact>();

                return Question.Rehydrate(
                    q.Id,
                    q.TemplateId,
                    q.Category,
                    q.Text,
                    q.Order,
                    q.CreatedAt,
                    q.UpdatedAt,
                    impacts
                );
            })
            .ToList();

        return ChecklistTemplate.Rehydrate(
            template.Id,
            template.Name,
            template.Description,
            template.IsActive,
            template.CreatedAt,
            template.UpdatedAt,
            questions
        );
    }

    private sealed record TemplateRow(
        Guid Id,
        string Name,
        string Description,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    private sealed record QuestionRow(
        Guid Id,
        Guid TemplateId,
        string Category,
        string Text,
        int Order,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    private sealed record ImpactRow(
        Guid QuestionId,
        Guid ContractModelId,
        int ImpactValue
    );

    public async Task<IReadOnlyList<ChecklistTemplate>> GetAsync(CancellationToken ct)
    {
        var templateRows = await _db.ChecklistTemplates
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new TemplateRow(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedAt,
                x.UpdatedAt
            ))
            .ToListAsync(ct);

        if (templateRows.Count == 0)
            return Array.Empty<ChecklistTemplate>();

        var templateIds = templateRows.Select(x => x.Id).ToList();

        var questionRows = await _db.Questions
            .AsNoTracking()
            .Where(x => templateIds.Contains(x.TemplateId))
            .OrderBy(x => x.TemplateId)
            .ThenBy(x => x.Order)
            .Select(x => new QuestionRow(
                x.Id,
                x.TemplateId,
                x.Category,
                x.Text,
                x.Order,
                x.CreatedAt,
                x.UpdatedAt
            ))
            .ToListAsync(ct);

        var questionIds = questionRows.Select(x => x.Id).ToList();

        List<ImpactRow> impactRows;

        if (questionIds.Count == 0)
        {
            impactRows = new List<ImpactRow>();
        }
        else
        {
            impactRows = await _db.QuestionImpacts
                .AsNoTracking()
                .Where(x => questionIds.Contains(x.QuestionId))
                .Select(x => new ImpactRow(
                    x.QuestionId,
                    x.ContractModelId,
                    x.ImpactValue
                ))
                .ToListAsync(ct);
        }

        var impactsByQuestionId = impactRows
            .GroupBy(x => x.QuestionId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyCollection<QuestionImpact>)g
                    .Select(i => QuestionImpact.Create(i.ContractModelId, i.ImpactValue))
                    .ToList()
            );

        var questionsByTemplateId = questionRows
            .Select(q =>
            {
                impactsByQuestionId.TryGetValue(q.Id, out var impacts);
                impacts ??= Array.Empty<QuestionImpact>();

                return Question.Rehydrate(
                    q.Id,
                    q.TemplateId,
                    q.Category,
                    q.Text,
                    q.Order,
                    q.CreatedAt,
                    q.UpdatedAt,
                    impacts
                );
            })
            .GroupBy(q => q.TemplateId)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<Question>)g.ToList());

        return templateRows
            .Select(t =>
            {
                questionsByTemplateId.TryGetValue(t.Id, out var questions);
                questions ??= Array.Empty<Question>();

                return ChecklistTemplate.Rehydrate(
                    t.Id,
                    t.Name,
                    t.Description,
                    t.IsActive,
                    t.CreatedAt,
                    t.UpdatedAt,
                    questions
                );
            })
            .ToList();
    }

    public async Task DeleteHardAsync(Guid id, CancellationToken ct)
    {
        if (id == Guid.Empty)
            return;

        var exists = await _db.ChecklistTemplates.AnyAsync(x => x.Id == id, ct);
        if (!exists)
            throw new KeyNotFoundException();

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        var questionIds = await _db.Questions
            .Where(x => x.TemplateId == id)
            .Select(x => x.Id)
            .ToListAsync(ct);

        if (questionIds.Count > 0)
        {
            await _db.QuestionImpacts
                .Where(x => questionIds.Contains(x.QuestionId))
                .ExecuteDeleteAsync(ct);

            await _db.Questions
                .Where(x => x.TemplateId == id)
                .ExecuteDeleteAsync(ct);
        }

        await _db.ChecklistTemplates
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(ct);

        await tx.CommitAsync(ct);
    }
}
