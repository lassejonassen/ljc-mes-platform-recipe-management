using RecipeManagement.Domain._Shared;
using RecipeManagement.Domain.MaterialDefinitions.Entities;

namespace RecipeManagement.Domain.MaterialDefinitions.Repositories;

public interface IMaterialDefinitionRepository : IRepository<MaterialDefinition>
{
    Task<IReadOnlyCollection<MaterialDefinition>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MaterialDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsSkuUniqueAsync(string sku, CancellationToken cancellationToken = default);
}
