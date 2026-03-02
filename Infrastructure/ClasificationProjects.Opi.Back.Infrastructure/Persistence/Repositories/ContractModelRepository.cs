using Microsoft.EntityFrameworkCore;
using ClasificationProjects.Opi.Back.Domain.Entities;
using ClasificationProjects.Opi.Back.Domain.Repositories;
using ClasificationProjects.Opi.Back.Infrastructure.Persistence;

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence.Repositories;

public sealed class ContractModelRepository : IContractModelRepository
{
    private readonly DbContext _db;

    public ContractModelRepository(DbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ContractModel>> GetAsync(CancellationToken ct)
    {
        var rows = await _db.ContractModels
            .AsNoTracking()
            .OrderBy(x => x.DisplayName)
            .Select(x => new { x.Id, x.Code, x.DisplayName })
            .ToListAsync(ct);

        return rows
            .Select(x => ContractModel.Create(x.Id, x.Code, x.DisplayName))
            .ToList();
    }
}

