using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;

namespace RecipeManagement.Application.ProcessSegments.Commands;

public sealed record DeleteProcessSegmentParameterCommand(Guid SegmentId, Guid ParameterId) : IRequest<Result>;

public sealed class DeleteProcessSegmentParameterCommandHandler(
    IProcessSegmentRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteProcessSegmentParameterCommand, Result>
{
    public async Task<Result> Handle(DeleteProcessSegmentParameterCommand request, CancellationToken cancellationToken)
    {
        var isParameterLinkedToProductParameter = await repository.IsParameterLinkedToAnyProductSegmentParameterAsync(request.ParameterId, cancellationToken);

        if (isParameterLinkedToProductParameter)
            return Result.Failure(ProcessSegmentErrors.ParameterLinkedToProduct);

        var processSegment = await repository.GetByIdAsync(request.SegmentId, cancellationToken);
            
        if (processSegment == null)
            return Result.Failure(ProcessSegmentErrors.NotFound);

        processSegment.RemoveParameter(request.ParameterId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}