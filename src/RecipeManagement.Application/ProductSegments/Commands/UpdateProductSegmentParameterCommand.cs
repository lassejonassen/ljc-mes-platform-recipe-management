using RecipeManagement.Domain.ProductSegments.Errors;
using RecipeManagement.Domain.ProductSegments.Repositories;

namespace RecipeManagement.Application.ProductSegments.Commands;

public sealed record UpdateProductSegmentParameterCommand(
    Guid ProductSegmentId,
    Guid ProductSegmentParameterId,
    string Value) : IRequest<Result>;

public sealed class UpdateProductSegmentParameterCommandHandler(
    IProductSegmentRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProductSegmentParameterCommand, Result>
{
    public async Task<Result> Handle(UpdateProductSegmentParameterCommand request, CancellationToken cancellationToken)
    {
        var productSegment = await repository.GetByIdAsync(request.ProductSegmentId, cancellationToken);

        if (productSegment is null)
            return Result.Failure(ProductSegmentErrors.NotFound);

        productSegment.UpdateParameter(request.ProductSegmentParameterId, request.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}