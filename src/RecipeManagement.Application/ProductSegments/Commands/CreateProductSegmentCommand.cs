using RecipeManagement.Domain.MaterialDefinitions.Errors;
using RecipeManagement.Domain.MaterialDefinitions.Repositories;
using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;
using RecipeManagement.Domain.ProductSegments.Aggregates;
using RecipeManagement.Domain.ProductSegments.Repositories;

namespace RecipeManagement.Application.ProductSegments.Commands;

public sealed record CreateProductSegmentCommand(Guid MaterialDefinitionId, Guid ProcessSegmentId) : IRequest<Result<Guid>>;

public sealed class CreateProductSegmentCommandHandler(
    IMaterialDefinitionRepository materialDefinitionRepository,
    IProcessSegmentRepository processSegmentRepository,
    IProductSegmentRepository productSegmentRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateProductSegmentCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductSegmentCommand request, CancellationToken cancellationToken)
    {
        var materialDefinition = await materialDefinitionRepository.GetByIdAsync(request.MaterialDefinitionId, cancellationToken);

        if (materialDefinition is null)
            return Result.Failure<Guid>(MaterialDefinitionErrors.NotFound);

        var processSegment = await processSegmentRepository.GetByIdAsync(request.ProcessSegmentId, cancellationToken);
        if (processSegment is null)
            return Result.Failure<Guid>(ProcessSegmentErrors.NotFound);

        var utcNow = dateTimeProvider.UtcNow;

        var productSegment = ProductSegment.Create(materialDefinition.Id, processSegment.Id, utcNow);

        if (productSegment.IsFailure)
            return Result.Failure<Guid>(productSegment.Error);

        foreach (var parameterTemplate in processSegment.Parameters)
        {
            productSegment.Value.AddParameter(parameterTemplate, utcNow);
        }

        productSegmentRepository.Add(productSegment.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(productSegment.Value.Id);
    }
}