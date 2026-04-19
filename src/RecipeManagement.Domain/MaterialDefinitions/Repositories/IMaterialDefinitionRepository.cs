using RecipeManagement.Domain.MaterialDefinitions.Aggregates;

namespace RecipeManagement.Domain.MaterialDefinitions.Repositories;

public interface IMaterialDefinitionRepository : IRepository<MaterialDefinition>
{
    Task<IReadOnlyCollection<MaterialDefinition>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MaterialDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsSkuUniqueAsync(string sku, CancellationToken cancellationToken = default);
    Task<int> GetLatestVersionAsync(string sku, CancellationToken cancellationToken = default);
}
