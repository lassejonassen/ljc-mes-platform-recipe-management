using RecipeManagement.Application.ProcessSegments.DTOs;
using RecipeManagement.Domain.ProcessSegments.Errors;
using RecipeManagement.Domain.ProcessSegments.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeManagement.Application.ProcessSegments.Queries;

public sealed record GetProcessSegmentByIdQuery(Guid Id) : IRequest<Result<ProcessSegmentDTO>>;

public sealed class GetProcessSegmentByIdQueryHandler(
    IProcessSegmentRepository repository)
    : IRequestHandler<GetProcessSegmentByIdQuery, Result<ProcessSegmentDTO>>
{
    public async Task<Result<ProcessSegmentDTO>> Handle(GetProcessSegmentByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
            return Result.Failure<ProcessSegmentDTO>(ProcessSegmentErrors.NotFound);

        var dto = new ProcessSegmentDTO
        {
            Id = entity.Id,
            CreatedAtUtc = entity.CreatedAtUtc,
            UpdatedAtUtc = entity.UpdatedAtUtc,
            Name = entity.Name,
            StableId = entity.StableId,
            State = entity.State.ToString(),
            Version = entity.Version,
            Parameters = entity.Parameters?.Select(p => new ProcessSegmentParameterDTO
            {
                Id = p.Id,
                CreatedAtUtc = p.CreatedAtUtc,
                UpdatedAtUtc = p.UpdatedAtUtc,
                Name = p.Name,
                Value = p.Value,
                IsReadOnly = p.IsReadOnly,
                ProcessSegmentId = entity.Id,
                DataType = p.DataType,
                Description = p.Description,
            }).ToList()
        };

        return Result.Success(dto);
    }
}