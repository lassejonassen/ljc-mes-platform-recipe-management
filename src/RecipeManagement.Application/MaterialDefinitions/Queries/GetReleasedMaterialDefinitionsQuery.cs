using RecipeManagement.Application.MaterialDefinitions.DTOs;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;

namespace RecipeManagement.Application.MaterialDefinitions.Queries;

public sealed record GetReleasedMaterialDefinitionsQuery : IRequest<IReadOnlyCollection<MaterialDefinitionDTO>>;

public sealed class GetReleasedMaterialDefinitionsQueryHandler(
    IMaterialDefinitionRepository repository)
    : IRequestHandler<GetReleasedMaterialDefinitionsQuery, IReadOnlyCollection<MaterialDefinitionDTO>>
{
    public async Task<IReadOnlyCollection<MaterialDefinitionDTO>> Handle(GetReleasedMaterialDefinitionsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetReleasedAsync(cancellationToken);

        return [.. entities.Select(e => new MaterialDefinitionDTO
        {
            Id = e.Id,
            Sku = e.Sku,
            Name = e.Name,
            State = e.State.ToString(),
            Version = e.Version,
            CreatedAtUtc = e.CreatedAtUtc,
            UpdatedAtUtc = e.UpdatedAtUtc,
        })];
    }
}