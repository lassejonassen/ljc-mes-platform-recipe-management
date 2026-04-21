using RecipeManagement.Domain.MaterialDefinitions.Errors;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;

namespace RecipeManagement.Application.MaterialDefinitions.Commands;

public sealed record CreateMaterialDefinitionDraftCommand(Guid Id) : IRequest<Result<Guid>>;

public sealed class CreateMaterialDefinitionDraftCommandHandler(
    IMaterialDefinitionRepository repository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateMaterialDefinitionDraftCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateMaterialDefinitionDraftCommand request, CancellationToken cancellationToken)
    {
        var materialDefinition = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (materialDefinition is null)
            return Result.Failure<Guid>(MaterialDefinitionErrors.NotFound);

        var latestVersion = await repository.GetLatestVersionAsync(materialDefinition.Sku, cancellationToken);

        if (materialDefinition.Version != latestVersion)
            return Result.Failure<Guid>(MaterialDefinitionErrors.NotLatestVersion);

        var newMaterialDefinition = materialDefinition.CreateDraft(dateTimeProvider.UtcNow);

        if (newMaterialDefinition.IsFailure)
            return Result.Failure<Guid>(newMaterialDefinition.Error);

        repository.Add(newMaterialDefinition.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(newMaterialDefinition.Value.Id);
    }
}