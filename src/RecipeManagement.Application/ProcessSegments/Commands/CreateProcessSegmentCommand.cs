using RecipeManagement.Domain.ProcessSegments.Aggregates;
using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;

namespace RecipeManagement.Application.ProcessSegments.Commands;

public sealed record CreateProcessSegmentCommand(string Name) : IRequest<Result<Guid>>;

public sealed class CreateProcessSegmentCommandHandler(
    IProcessSegmentRepository repository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateProcessSegmentCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProcessSegmentCommand request, CancellationToken cancellationToken)
    {
        bool isNameInUse = await repository.IsNameUniqueAsync(request.Name, cancellationToken);

        if (!isNameInUse) return Result.Failure<Guid>(ProcessSegmentErrors.NameIsAlreadyInUse);

        var processSegmentResult = ProcessSegment.Create(request.Name, dateTimeProvider.UtcNow);

        if (processSegmentResult.IsFailure)
            return Result.Failure<Guid>(processSegmentResult.Error);


        repository.Add(processSegmentResult.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(processSegmentResult.Value.Id);
    }
}
