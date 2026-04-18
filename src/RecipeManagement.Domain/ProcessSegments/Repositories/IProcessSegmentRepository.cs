using RecipeManagement.Domain.ProcessSegments.Aggregates;

namespace RecipeManagement.Domain.ProcessSegments.Repositories;

public interface IProcessSegmentRepository : IRepository<ProcessSegment>
{
    Task<IReadOnlyCollection<ProcessSegment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProcessSegment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsNameUniqueAsync(string sku, CancellationToken cancellationToken = default);
    Task<bool> IsLinkedToAnyProductSegmentAsync(Guid processSegmentId, CancellationToken cancellationToken = default);
    Task<bool> IsParameterLinkedToAnyProductSegmentParameterAsync(Guid processSegmentParamterId, CancellationToken cancellationToken = default);
}
