using Microsoft.EntityFrameworkCore;
using RecipeManagement.Domain.MaterialDefinitions.Aggregates;
using RecipeManagement.Domain.ProcessSegments.Aggregates;
using RecipeManagement.Domain.ProcessSegments.Enums;
using RecipeManagement.Domain.ProcessSegments.Repositories;
using RecipeManagement.Infrastructure.Persistence.DbContexts;

namespace RecipeManagement.Infrastructure.Persistence.Repositories;

internal sealed class ProcessSegmentRepository
    (ApplicationDbContext context)
     : Repository<ProcessSegment>(context), IProcessSegmentRepository
{
    public async Task<IReadOnlyCollection<ProcessSegment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ProcessSegment>().ToListAsync(cancellationToken);
    }

    public async Task<ProcessSegment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ProcessSegment>()
           .Include(x => x.Parameters)
           .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<int> GetLatestVersionAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ProcessSegment>()
            .Where(m => m.Name == name)
            .OrderByDescending(m => m.Version)
            .Select(m => m.Version)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProcessSegment?> GetReleasedAsync(Guid stableId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ProcessSegment>()
            .Include(x => x.Parameters)
            .FirstOrDefaultAsync(x => x.StableId == stableId && x.State == ProcessSegmentState.Released, cancellationToken);
    }

    public async Task<IReadOnlyCollection<ProcessSegment>> GetReleasedAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ProcessSegment>()
            .Where(x => x.State == ProcessSegmentState.Released)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsLinkedToAnyProductSegmentAsync(Guid processSegmentId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ProcessSegment>()
            .Where(x => x.Id == processSegmentId)
            .SelectMany(x => x.ProductSegments)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> IsNameUniqueAsync(string name, CancellationToken cancellationToken = default)
    {
        return !await DbContext.Set<ProcessSegment>().AnyAsync(x => x.Name == name, cancellationToken);  
    }

    public async Task<bool> IsParameterLinkedToAnyProductSegmentParameterAsync(Guid processSegmentParamterId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ProcessSegment>()
            .SelectMany(x => x.Parameters)
            .Where(x => x.Id == processSegmentParamterId)
            .SelectMany(x => x.ProductSegmentParameters)
            .AnyAsync(cancellationToken);
    }
}
