using RecipeManagement.Domain.MaterialDefinitions.Errors;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;

namespace RecipeManagement.Application.MaterialDefinitions.Commands;

public sealed record DeprecateMaterialDefinitionCommand(Guid Id) : IRequest<Result>;

public sealed class DeprecateMaterialDefinitionCommandHandler(
    IMaterialDefinitionRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeprecateMaterialDefinitionCommand, Result>
{
    public async Task<Result> Handle(DeprecateMaterialDefinitionCommand request, CancellationToken cancellationToken)
    {
        var materialDefinition = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (materialDefinition is null)
            return Result.Failure(MaterialDefinitionErrors.NotFound);

        materialDefinition.Deprecate();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
