using Microsoft.EntityFrameworkCore;
using RecipeManagement.Domain.ProductSegments.Aggregates;
using RecipeManagement.Domain.ProductSegments.Repositories;
using RecipeManagement.Infrastructure.Persistence.DbContexts;

namespace RecipeManagement.Infrastructure.Persistence.Repositories;

internal sealed class ProductSegmentRepository
    (ApplicationDbContext context)
     : Repository<ProductSegment>(context), IProductSegmentRepository
{
    public async Task<IReadOnlyCollection<ProductSegment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ProductSegment>()
            .Include(x => x.MaterialDefinition)
            .Include(x => x.ProcessSegment)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductSegment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ProductSegment>()
           .Include(x => x.Parameters)
           .Include(x => x.MaterialDefinition)
           .Include(x => x.ProcessSegment)
           .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<int> GetLatestVersionAsync(Guid stableId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ProductSegment>()
            .Where(m => m.StableId == stableId)
            .OrderByDescending(m => m.Version)
            .Select(m => m.Version)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
