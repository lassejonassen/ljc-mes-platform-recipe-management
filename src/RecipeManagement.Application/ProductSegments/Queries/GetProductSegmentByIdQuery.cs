using RecipeManagement.Application.ProductSegments.DTOs;
using RecipeManagement.Domain.ProductSegments.Errors;
using RecipeManagement.Domain.ProductSegments.Repositories;

namespace RecipeManagement.Application.ProductSegments.Queries;

public sealed record GetProductSegmentByIdQuery(Guid Id) : IRequest<Result<ProductSegmentDTO>>;

public sealed class GetProductSegmentByIdQueryHandler(
    IProductSegmentRepository repository)
    : IRequestHandler<GetProductSegmentByIdQuery, Result<ProductSegmentDTO>>
{
    public async Task<Result<ProductSegmentDTO>> Handle(GetProductSegmentByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
            return Result.Failure<ProductSegmentDTO>(ProductSegmentErrors.NotFound);

        var dto = new ProductSegmentDTO
        {
            Id = entity.Id,
            MaterialDefinitionId = entity.MaterialDefinitionId,
            ProcessSegmentId = entity.ProcessSegmentId,
            MaterialSku = entity.MaterialDefinition.Sku,
            MaterialName = entity.MaterialDefinition.Name,
            ProcessSegmentName = entity.ProcessSegment.Name,
            State = entity.State.ToString(),
            Version = entity.Version,
            Parameters = entity.Parameters?.Select(p => new ProductSegmentParameterDTO
            {
                Id = p.Id,
                Name = p.Name,
                Value = p.Value,
                DataType = p.DataType,
                Description = p.Description,
            }).ToList()
        };

        return Result.Success(dto);
    }
}