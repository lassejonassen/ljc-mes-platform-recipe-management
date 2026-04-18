using RecipeManagement.Domain.MaterialDefinitions.Errors;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;
using RecipeManagement.Domain.ProcessSegments.Errors;

namespace RecipeManagement.Application.MaterialDefinitions.Commands;

public sealed record DeleteMaterialDefinitionPropertyCommand(
    Guid MaterialDefinitionId,
    Guid PropertyId) : IRequest<Result>;

public sealed class DeleteMaterialDefinitionPropertyCommandHandler(
    IMaterialDefinitionRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteMaterialDefinitionPropertyCommand, Result>
{
    public async Task<Result> Handle(DeleteMaterialDefinitionPropertyCommand request, CancellationToken cancellationToken)
    {
        var materialDefinition = await repository.GetByIdAsync(request.MaterialDefinitionId, cancellationToken);
        if (materialDefinition == null)
            return Result.Failure(MaterialDefinitionErrors.NotFound);

        var result = materialDefinition.RemoveProperty(request.PropertyId);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}