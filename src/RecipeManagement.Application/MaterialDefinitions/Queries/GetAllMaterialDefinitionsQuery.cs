using RecipeManagement.Application.MaterialDefinitions.DTOs;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;

namespace RecipeManagement.Application.MaterialDefinitions.Queries;

public sealed record GetAllMaterialDefinitionsQuery : IRequest<List<MaterialDefinitionDTO>>;

public sealed record GetAllMaterialDefinitionsQueryHandler(IMaterialDefinitionRepository repository)
    : IRequestHandler<GetAllMaterialDefinitionsQuery, List<MaterialDefinitionDTO>>
{
    public async Task<List<MaterialDefinitionDTO>> Handle(GetAllMaterialDefinitionsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);

        return [.. entities.Select(e => new MaterialDefinitionDTO
        {
            Id = e.Id,
            Sku = e.Sku,
            Name = e.Name,
            CreatedAtUtc = e.CreatedAtUtc,
            UpdatedAtUtc = e.UpdatedAtUtc,
        })];
    }
}