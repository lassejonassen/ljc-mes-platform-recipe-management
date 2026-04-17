using RecipeManagement.Domain.MaterialDefinitions.Errors;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;

namespace RecipeManagement.Application.MaterialDefinitions.Commands;

public sealed record UpdateMaterialDefinitionCommand(Guid Id, string Name) : IRequest<Result>;

public sealed class UpdateMaterialDefinitionCommandHandler(
    IMaterialDefinitionRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMaterialDefinitionCommand, Result>
{
    public async Task<Result> Handle(UpdateMaterialDefinitionCommand request, CancellationToken cancellationToken)
    {
        var materialDefinition = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (materialDefinition is null)
            return Result.Failure(MaterialDefinitionErrors.NotFound);

        var updateResult = materialDefinition.Rename(request.Name);

        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}