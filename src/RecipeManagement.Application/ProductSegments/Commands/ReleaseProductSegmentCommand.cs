using RecipeManagement.Domain.ProductSegments.Errors;
using RecipeManagement.Domain.ProductSegments.Repositories;

namespace RecipeManagement.Application.ProductSegments.Commands;

public sealed record ReleaseProductSegmentCommand(Guid Id) : IRequest<Result>;

public sealed class ReleaseProductSegmentCommandHandler(
    IProductSegmentRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ReleaseProductSegmentCommand, Result>
{
    public async Task<Result> Handle(ReleaseProductSegmentCommand request, CancellationToken cancellationToken)
    {
        var productSegment = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (productSegment is null)
            return Result.Failure(ProductSegmentErrors.NotFound);

        productSegment.Release();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}