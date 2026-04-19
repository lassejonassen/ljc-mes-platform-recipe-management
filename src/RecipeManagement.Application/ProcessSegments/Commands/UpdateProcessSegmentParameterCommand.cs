using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;

namespace RecipeManagement.Application.ProcessSegments.Commands;

public sealed record UpdateProcessSegmentParameterCommand(
    Guid ProcessSegmentId,
    Guid ParameterId,
    string Name,
    string Value,
    string? DataType,
    string? Description,
    bool IsReadOnly) : IRequest<Result>;

public sealed class UpdateProcessSegmentParameterCommandHandler(
    IProcessSegmentRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProcessSegmentParameterCommand, Result>
{
    public async Task<Result> Handle(UpdateProcessSegmentParameterCommand request, CancellationToken cancellationToken)
    {
        var processSegment = await repository.GetByIdAsync(request.ProcessSegmentId, cancellationToken);

        if (processSegment is null)
            return Result.Failure(ProcessSegmentErrors.NotFound);

        var updateResult = processSegment.UpdateParameter(
            request.ParameterId,
            request.Name,
            request.Value,
            request.DataType,
            request.Description,
            request.IsReadOnly);

        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}