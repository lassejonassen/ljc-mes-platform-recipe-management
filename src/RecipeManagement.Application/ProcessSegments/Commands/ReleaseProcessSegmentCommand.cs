using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;

namespace RecipeManagement.Application.ProcessSegments.Commands;

public sealed record ReleaseProcessSegmentCommand(Guid Id) : IRequest<Result>;

public sealed class ReleaseProcessSegmentCommandHandler(
    IProcessSegmentRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ReleaseProcessSegmentCommand, Result>
{
    public async Task<Result> Handle(ReleaseProcessSegmentCommand request, CancellationToken cancellationToken)
    {
        var processSegment = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (processSegment is null)
            return Result.Failure(ProcessSegmentErrors.NotFound);

        processSegment.Release();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}