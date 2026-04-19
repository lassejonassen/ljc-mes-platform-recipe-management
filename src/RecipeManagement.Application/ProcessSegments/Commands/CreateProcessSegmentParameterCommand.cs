using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;

namespace RecipeManagement.Application.ProcessSegments.Commands;

public sealed record CreateProcessSegmentParameterCommand(
    Guid ProcessSegmentId,
    string Name,
    string Value,
    string? DataType,
    string? Description,
    bool IsReadOnly,
    string DefaultValue) : IRequest<Result>;

public sealed class CreateProcessSegmentParameterCommandHandler(
    IProcessSegmentRepository repository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateProcessSegmentParameterCommand, Result>
{
    public async Task<Result> Handle(CreateProcessSegmentParameterCommand request, CancellationToken cancellationToken)
    {
        var processSegment = await repository.GetByIdAsync(request.ProcessSegmentId, cancellationToken);
        if (processSegment is null) 
            return Result.Failure(ProcessSegmentErrors.NotFound);

        var result = processSegment.AddParameter(
            request.Name,
            request.Value,
            request.DataType,
            request.Description,
            request.IsReadOnly,
            request.DefaultValue,
            dateTimeProvider.UtcNow);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
