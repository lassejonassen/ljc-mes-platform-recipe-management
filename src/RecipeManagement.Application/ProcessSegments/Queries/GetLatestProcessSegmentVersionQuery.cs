using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;

namespace RecipeManagement.Application.ProcessSegments.Queries;

public sealed record GetLatestProcessSegmentVersionQuery(Guid Id) : IRequest<Result<int>>;

public sealed class GetLatestProcessSegmentVersionQueryHandler(
    IProcessSegmentRepository repository)
    : IRequestHandler<GetLatestProcessSegmentVersionQuery, Result<int>>
{
    public async Task<Result<int>> Handle(GetLatestProcessSegmentVersionQuery request, CancellationToken cancellationToken)
    {
        var processSegment = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (processSegment is null)
            return Result.Failure<int>(ProcessSegmentErrors.NotFound);

        int latestVersion = await repository.GetLatestVersionAsync(processSegment.Name, cancellationToken);

        return Result.Success(latestVersion);
    }
}