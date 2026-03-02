using MediatR;
using ClasificationProjects.Opi.Back.Domain.Repositories;

namespace ClasificationProjects.Opi.Back.Application.Features.ContractModels.Queries.Get;

public sealed class GetContractModelsHandler
    : IRequestHandler<GetContractModelsQuery, List<ContractModelDto>>
{
    private readonly IContractModelRepository _repo;

    public GetContractModelsHandler(IContractModelRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ContractModelDto>> Handle(GetContractModelsQuery request, CancellationToken ct)
    {
        var models = await _repo.GetAsync(ct);

        return models
            .OrderBy(x => x.DisplayName)
            .Select(x => new ContractModelDto(
                x.Id,
                x.Code,
                x.DisplayName
            ))
            .ToList();
    }
}