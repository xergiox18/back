using MediatR;

namespace ClasificationProjects.Opi.Back.Application.Features.ContractModels.Queries.Get;

public sealed record GetContractModelsQuery() : IRequest<List<ContractModelDto>>;

public sealed record ContractModelDto(
    Guid Id,
    string Code,
    string DisplayName
);

