using RecipeManagement.Application.ProcessSegments.DTOs;
using RecipeManagement.Domain.ProcessSegments.Repositories;

namespace RecipeManagement.Application.ProcessSegments.Queries;

public sealed record GetReleasedProcessSegmentsQuery : IRequest<IReadOnlyCollection<ProcessSegmentDTO>>;

public sealed class GetReleasedProcessSegmentsQueryHandler(
    IProcessSegmentRepository repository)
    : IRequestHandler<GetReleasedProcessSegmentsQuery, IReadOnlyCollection<ProcessSegmentDTO>>
{
    public async Task<IReadOnlyCollection<ProcessSegmentDTO>> Handle(GetReleasedProcessSegmentsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetReleasedAsync(cancellationToken);

        return [.. entities.Select(e => new ProcessSegmentDTO {
            Id = e.Id,
            Name = e.Name,
            StableId = e.StableId,
            State = e.State.ToString(),
            Version = e.Version,
            CreatedAtUtc = e.CreatedAtUtc,
            UpdatedAtUtc = e.UpdatedAtUtc,
        })];
    }
}