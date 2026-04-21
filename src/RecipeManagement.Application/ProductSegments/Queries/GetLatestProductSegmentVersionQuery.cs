using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProductSegments.Repositories;

namespace RecipeManagement.Application.ProductSegments.Queries;

public sealed record GetLatestProductSegmentVersionQuery(Guid Id) : IRequest<Result<int>>;

public sealed class GetLatestProductSegmentVersionQueryHandler(
    IProductSegmentRepository repository)
    : IRequestHandler<GetLatestProductSegmentVersionQuery, Result<int>>
{
    public async Task<Result<int>> Handle(GetLatestProductSegmentVersionQuery request, CancellationToken cancellationToken)
    {
        var productSegment = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (productSegment is null)
            return Result.Failure<int>(ProcessSegmentErrors.NotFound);

        int latestVersion = await repository.GetLatestVersionAsync(productSegment.StableId, cancellationToken);

        return Result.Success(latestVersion);
    }
}