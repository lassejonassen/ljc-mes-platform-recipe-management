using RecipeManagement.Application.ProductSegments.DTOs;
using RecipeManagement.Domain.ProductSegments.Repositories;

namespace RecipeManagement.Application.ProductSegments.Queries;

public sealed record GetAllProductSegmentsQuery : IRequest<IReadOnlyCollection<ProductSegmentDTO>>;

public sealed class GetAllProductSegmentsQueryHandler(
    IProductSegmentRepository repository)
    : IRequestHandler<GetAllProductSegmentsQuery, IReadOnlyCollection<ProductSegmentDTO>>
{
    public async Task<IReadOnlyCollection<ProductSegmentDTO>> Handle(GetAllProductSegmentsQuery request, CancellationToken cancellationToken)
    {
        var productSegments = await repository.GetAllAsync(cancellationToken);

        return [.. productSegments.Select(e => new ProductSegmentDTO {
            Id = e.Id,
            MaterialDefinitionId = e.MaterialDefinitionId,
            ProcessSegmentId = e.ProcessSegmentId,
            ProcessSegmentName = e.ProcessSegment.Name,
            MaterialSku = e.MaterialDefinition.Sku,
            MaterialName = e.MaterialDefinition.Name,
            State = e.State.ToString(),
            Version = e.Version
        })];
    }
}