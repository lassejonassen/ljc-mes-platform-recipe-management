using RecipeManagement.Domain.MaterialDefinitions.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;

namespace RecipeManagement.Application.ProcessSegments.Commands;

public sealed record DeprecateProcessSegmentCommand(Guid Id) : IRequest<Result>;

public sealed class DeprecateProcessSegmentCommandHandler(
    IProcessSegmentRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeprecateProcessSegmentCommand, Result>
{
    public async Task<Result> Handle(DeprecateProcessSegmentCommand request, CancellationToken cancellationToken)
    {
        var materialDefinition = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (materialDefinition is null)
            return Result.Failure(MaterialDefinitionErrors.NotFound);

        materialDefinition.Deprecate();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
