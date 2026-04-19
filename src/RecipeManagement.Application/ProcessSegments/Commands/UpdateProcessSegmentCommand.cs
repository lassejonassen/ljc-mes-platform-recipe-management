using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;

namespace RecipeManagement.Application.ProcessSegments.Commands;

public sealed record UpdateProcessSegmentCommand(Guid ProcessSegmentId, string Name) : IRequest<Result>;

public sealed class UpdateProcessSegmentCommandHandler(
    IProcessSegmentRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProcessSegmentCommand, Result>
{
    public async Task<Result> Handle(UpdateProcessSegmentCommand request, CancellationToken cancellationToken)
    {
        var processSegment = await repository.GetByIdAsync(request.ProcessSegmentId, cancellationToken);

        if (processSegment is null)
            return Result.Failure(ProcessSegmentErrors.NotFound);

        if (processSegment.Name == request.Name)
            return Result.Success();

        bool isNameInUse = await repository.IsNameUniqueAsync(request.Name, cancellationToken);

        if (isNameInUse)
            return Result.Failure(ProcessSegmentErrors.NameIsAlreadyInUse);

        processSegment.Rename(request.Name);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}