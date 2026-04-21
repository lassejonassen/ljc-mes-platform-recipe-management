using RecipeManagement.Domain.ProductSegments.Errors;
using RecipeManagement.Domain.ProductSegments.Repositories;

namespace RecipeManagement.Application.ProductSegments.Commands;

public sealed record DeprecateProductSegmentCommand(Guid Id) : IRequest<Result>;

public sealed class DeprecateProductSegmentCommandHandler(
    IProductSegmentRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeprecateProductSegmentCommand, Result>
{
    public async Task<Result> Handle(DeprecateProductSegmentCommand request, CancellationToken cancellationToken)
    {
        var productSegment = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (productSegment is null)
            return Result.Failure(ProductSegmentErrors.NotFound);

        productSegment.Deprecate();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
