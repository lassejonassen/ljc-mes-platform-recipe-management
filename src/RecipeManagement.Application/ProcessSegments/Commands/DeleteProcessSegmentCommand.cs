using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;

namespace RecipeManagement.Application.ProcessSegments.Commands;

public sealed record DeleteProcessSegmentCommand(Guid Id) : IRequest<Result>;

public sealed class DeleteProcessSegmentCommandHandler(
    IProcessSegmentRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteProcessSegmentCommand, Result>
{
    public async Task<Result> Handle(DeleteProcessSegmentCommand request, CancellationToken cancellationToken)
    {
        var isLinkedToProduct = await repository.IsLinkedToAnyProductSegmentAsync(request.Id, cancellationToken);

        if (isLinkedToProduct)
            return Result.Failure(ProcessSegmentErrors.LinkedToProduct);

        var processSegment = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (processSegment == null)
            return Result.Failure(ProcessSegmentErrors.NotFound);

        repository.Delete(processSegment);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}