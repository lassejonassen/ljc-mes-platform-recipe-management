using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;

namespace RecipeManagement.Application.ProcessSegments.Commands;

public sealed record CreateProcessSegmentDraftCommand(Guid Id) : IRequest<Result<Guid>>;

public sealed class CreateProcessSegmentDraftCommandHandler(
    IProcessSegmentRepository repository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateProcessSegmentDraftCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProcessSegmentDraftCommand request, CancellationToken cancellationToken)
    {
        var processSegment = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (processSegment is null)
            return Result.Failure<Guid>(ProcessSegmentErrors.NotFound);

        var latestVersion = await repository.GetLatestVersionAsync(processSegment.Name, cancellationToken);

        if (processSegment.Version != latestVersion)
            return Result.Failure<Guid>(ProcessSegmentErrors.NotLatestVersion);

        var newProcessSegment = processSegment.CreateDraft(dateTimeProvider.UtcNow);

        if (newProcessSegment.IsFailure)
            return Result.Failure<Guid>(newProcessSegment.Error);

        repository.Add(newProcessSegment.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(newProcessSegment.Value.Id);
    }
}