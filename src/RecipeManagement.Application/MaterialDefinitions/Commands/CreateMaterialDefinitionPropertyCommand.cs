using RecipeManagement.Domain.MaterialDefinitions.Errors;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;
using RecipeManagement.Domain.ProcessSegments.Errors;

namespace RecipeManagement.Application.MaterialDefinitions.Commands;
public sealed record CreateMaterialDefinitionPropertyCommand(
    Guid MaterialDefinitionId,
    string Name,
    string Value,
    string? DataType,
    string? Description) : IRequest<Result>;


public sealed class CreateMaterialDefinitionPropertyCommandHandler(
    IMaterialDefinitionRepository repository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<CreateMaterialDefinitionPropertyCommand, Result>
{
    public async Task<Result> Handle(CreateMaterialDefinitionPropertyCommand request, CancellationToken cancellationToken)
    {
        var materialDefinition = await repository.GetByIdAsync(request.MaterialDefinitionId, cancellationToken);

        if (materialDefinition == null)
            return Result.Failure(MaterialDefinitionErrors.NotFound);

        var result = materialDefinition.AddProperty(request.Name, request.Value, request.DataType, request.Description, dateTimeProvider.UtcNow);
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value);
    }
}