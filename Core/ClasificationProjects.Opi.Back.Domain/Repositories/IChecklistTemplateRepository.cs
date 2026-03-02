using ClasificationProjects.Opi.Back.Domain.Aggregates;
using ClasificationProjects.Opi.Back.Domain.Entities;

namespace ClasificationProjects.Opi.Back.Domain.Repositories;

public interface IChecklistTemplateRepository
{
    Task AddAsync(ChecklistTemplate aggregate, CancellationToken ct);
    Task UpdateAsync(ChecklistTemplate aggregate, CancellationToken ct);
    Task<ChecklistTemplate?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<ChecklistTemplate>> GetAsync(CancellationToken ct);
    Task DeleteHardAsync(Guid id, CancellationToken ct);
}

public interface IQuestionRepository
{
    Task AddManyAsync(Guid templateId, IReadOnlyList<Question> questions, CancellationToken ct);
    Task<Question?> GetByIdAsync(Guid id, CancellationToken ct);
    Task UpdateAsync(Question entity, CancellationToken ct);
    Task DeleteHardAsync(Guid id, CancellationToken ct);
}

public interface IContractModelRepository
{
    Task<IReadOnlyList<ContractModel>> GetAsync(CancellationToken ct);
}

public interface IEvaluationRepository
{
    Task AddAsync(Evaluation aggregate, CancellationToken ct);
    Task<Evaluation?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Evaluation>> GetAsync(CancellationToken ct);
    Task<IReadOnlyList<Evaluation>> GetByTemplateAsync(Guid templateId, CancellationToken ct);
    Task DeleteHardAsync(Guid id, CancellationToken ct);
}