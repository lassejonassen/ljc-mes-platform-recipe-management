using RecipeManagement.Domain.MaterialDefinitions.Aggregates;
using RecipeManagement.Domain.MaterialDefinitions.Errors;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;
using RecipeManagement.Domain.ProcessSegments.Errors;

namespace RecipeManagement.Application.MaterialDefinitions.Commands;

public sealed record CreateMaterialDefinitionCommand(string Sku, string Name) : IRequest<Result<Guid>>;

public sealed class CreateMaterialDefinitionCommandHandler(
    IMaterialDefinitionRepository repository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateMaterialDefinitionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateMaterialDefinitionCommand request, CancellationToken cancellationToken)
    {
        var isSkuUnique = await repository.IsSkuUniqueAsync(request.Sku, cancellationToken);

        if (!isSkuUnique)
            return Result.Failure<Guid>(MaterialDefinitionErrors.SkuAlreadyExists);

        var materialDefinition = MaterialDefinition.Create(request.Sku, request.Name, dateTimeProvider.UtcNow);

        if (materialDefinition.IsFailure)
            return Result.Failure<Guid>(materialDefinition.Error);

        repository.Add(materialDefinition.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(materialDefinition.Value.Id);
    }
}