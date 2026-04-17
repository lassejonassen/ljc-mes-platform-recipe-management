using RecipeManagement.Domain.MaterialDefinitions.Errors;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;
using RecipeManagement.SharedKernel;
using RecipeManagement.SharedKernel.Messaging;

namespace RecipeManagement.Application.MaterialDefinitions.Commands;

public sealed record UpdateMaterialDefinitionPropertyCommand(
    Guid MaterialDefinitionId,
    Guid PropertyId,
    string Name,
    string Value,
    string? DataType,
    string? Description) : IRequest<Result>;

public sealed class UpdateMaterialDefinitionPropertyCommandHandler(
    IMaterialDefinitionRepository repository,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateMaterialDefinitionPropertyCommand, Result>
{
    public async Task<Result> Handle(UpdateMaterialDefinitionPropertyCommand request, CancellationToken cancellationToken)
    {
        var materialDefinition = await repository.GetByIdAsync(request.MaterialDefinitionId, cancellationToken);

        if (materialDefinition == null)
            return Result.Failure(MaterialDefinitionErrors.NotFound);

        var result = materialDefinition.UpdateProperty(request.Name, request.Value, request.DataType, request.Description);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
