using RecipeManagement.Application.MaterialDefinitions.DTOs;
using RecipeManagement.Domain.MaterialDefinitions.Errors;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;
using RecipeManagement.SharedKernel;
using RecipeManagement.SharedKernel.Messaging;

namespace RecipeManagement.Application.MaterialDefinitions.Queries;

public sealed record GetMaterialDefinitionByIdQuery(Guid Id) : IRequest<Result<MaterialDefinitionDTO>>;

public sealed class GetMaterialDefinitionByIdQueryHandler(IMaterialDefinitionRepository repository)
    : IRequestHandler<GetMaterialDefinitionByIdQuery, Result<MaterialDefinitionDTO>>
{
    public async Task<Result<MaterialDefinitionDTO>> Handle(GetMaterialDefinitionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return Result.Failure<MaterialDefinitionDTO>(MaterialDefinitionErrors.NotFound);
        }
        var dto = new MaterialDefinitionDTO
        {
            Id = entity.Id,
            Sku = entity.Sku,
            Name = entity.Name,
            CreatedAtUtc = entity.CreatedAtUtc,
            UpdatedAtUtc = entity.UpdatedAtUtc,
            Properties = entity.Properties?.Select(p => new MaterialDefinitionPropertyDTO
            {
                Id = p.Id,
                Name = p.Name,
                Value = p.Value,
                DataType = p.DataType,
                Description = p.Description,
                CreatedAtUtc = p.CreatedAtUtc,
                UpdatedAtUtc = p.UpdatedAtUtc
            }).ToList()
        };
        return Result.Success(dto);
    }
}