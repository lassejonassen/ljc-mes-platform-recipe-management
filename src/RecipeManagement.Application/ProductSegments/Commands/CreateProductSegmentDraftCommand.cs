using RecipeManagement.Domain._Shared;
using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;
using RecipeManagement.Domain.ProductSegments.Errors;
using RecipeManagement.Domain.ProductSegments.Repositories;

namespace RecipeManagement.Application.ProductSegments.Commands;

public sealed record CreateProductSegmentDraftCommand(Guid Id) : IRequest<Result<Guid>>;

public sealed class CreateProductSegmentDraftCommandHandler(
    IProductSegmentRepository productSegmentRepository,
    IProcessSegmentRepository processSegmentRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateProductSegmentDraftCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductSegmentDraftCommand request, CancellationToken cancellationToken)
    {
        var productSegment = await productSegmentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (productSegment is null)
            return Result.Failure<Guid>(ProductSegmentErrors.NotFound);

        var processSegment = await processSegmentRepository.GetReleasedAsync(productSegment.ProcessSegment.StableId, cancellationToken);

        if (processSegment is null)
            return Result.Failure<Guid>(ProductSegmentErrors.ProcessSegmentNotReleased);

        var utcNow = dateTimeProvider.UtcNow;

        var latestVersion = await productSegmentRepository.GetLatestVersionAsync(productSegment.StableId, cancellationToken);

        if (productSegment.Version != latestVersion)
            return Result.Failure<Guid>(ProductSegmentErrors.NotLatestVersion);

        var newProductSegment = productSegment.CreateDraft(processSegment.Id, utcNow);

        if(newProductSegment.IsFailure)
            return Result.Failure<Guid>(newProductSegment.Error);

        foreach (var parameterTemplate in processSegment.Parameters)
        {
            newProductSegment.Value.AddParameter(parameterTemplate, utcNow);
        }

        productSegmentRepository.Add(newProductSegment.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(newProductSegment.Value.Id);
    }
}