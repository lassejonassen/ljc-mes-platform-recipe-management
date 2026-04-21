using Microsoft.EntityFrameworkCore;
using RecipeManagement.Domain.MaterialDefinitions.Aggregates;
using RecipeManagement.Domain.MaterialDefinitions.Enums;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;
using RecipeManagement.Infrastructure.Persistence.DbContexts;

namespace RecipeManagement.Infrastructure.Persistence.Repositories;

internal sealed class MaterialDefinitionRepository(ApplicationDbContext context)
     : Repository<MaterialDefinition>(context), IMaterialDefinitionRepository
{
    public async Task<IReadOnlyCollection<MaterialDefinition>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<MaterialDefinition>().ToListAsync(cancellationToken);
    }

    public async Task<MaterialDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<MaterialDefinition>()
            .Include(x => x.Properties)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<int> GetLatestVersionAsync(string sku, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<MaterialDefinition>()
            .Where(m => m.Sku == sku)
            .OrderByDescending(m => m.Version)
            .Select(m => m.Version)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<MaterialDefinition>> GetReleasedAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<MaterialDefinition>()
            .Where(x => x.State == MaterialDefinitionState.Released)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsSkuUniqueAsync(string sku, CancellationToken cancellationToken = default)
    {
        return !await DbContext.Set<MaterialDefinition>().AnyAsync(x => x.Sku == sku, cancellationToken);
    }
}
