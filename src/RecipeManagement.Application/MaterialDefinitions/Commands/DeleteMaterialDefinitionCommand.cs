using RecipeManagement.Domain.MaterialDefinitions.Errors;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;

namespace RecipeManagement.Application.MaterialDefinitions.Commands;

public sealed record DeleteMaterialDefinitionCommand(Guid Id) : IRequest<Result>;

public sealed class DeleteMaterialDefinitionCommandHandler(
    IMaterialDefinitionRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteMaterialDefinitionCommand, Result>
{
    public async Task<Result> Handle(DeleteMaterialDefinitionCommand request, CancellationToken cancellationToken)
    {
        var materialDefinition = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (materialDefinition is null)
            return Result.Failure(MaterialDefinitionErrors.NotFound);

        repository.Delete(materialDefinition);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}