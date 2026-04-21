using RecipeManagement.Domain.ProductSegments.Aggregates;

namespace RecipeManagement.Domain.ProductSegments.Repositories;

public interface IProductSegmentRepository : IRepository<ProductSegment>
{
    Task<IReadOnlyCollection<ProductSegment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductSegment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> GetLatestVersionAsync(Guid stableId, CancellationToken cancellationToken = default);
}
