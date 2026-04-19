using RecipeManagement.Domain.ProductSegments.Errors;
using RecipeManagement.Domain.ProductSegments.Repositories;

namespace RecipeManagement.Application.ProductSegments.Commands;

public sealed record DeleteProductSegmentCommand(Guid Id) : IRequest<Result>;

public sealed class DeleteProductSegmentCommandHandler(
    IProductSegmentRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProductSegmentCommand, Result>
{
    public async Task<Result> Handle(DeleteProductSegmentCommand request, CancellationToken cancellationToken)
    {
        var productSegment = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (productSegment is null)
            return Result.Failure(ProductSegmentErrors.NotFound);

        repository.Delete(productSegment);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
