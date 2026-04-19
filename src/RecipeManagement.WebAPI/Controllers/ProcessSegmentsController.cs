using Microsoft.AspNetCore.Mvc;
using RecipeManagement.Application.MaterialDefinitions.Commands;
using RecipeManagement.Application.ProcessSegments.Commands;
using RecipeManagement.Application.ProcessSegments.Queries;
using RecipeManagement.WebAPI.Contracts.MaterialDefinitions;
using RecipeManagement.WebAPI.Contracts.ProcessSegments;
using RecipeManagement.WebAPI.Contracts.ProductSegments;

namespace RecipeManagement.WebAPI.Controllers;

[Route("api/process-segments")]
public class ProcessSegmentsController : BaseController
{
    [ProducesResponseType(typeof(ProcessSegmentListResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllProcessSegmentsQuery();

        var result = await Mediator.Send(query, cancellationToken);

        if (!result.Any())
        {
            return NoContent();
        }

        var dtos = result.Select(x => new ProcessSegmentResponseDTO
        {
            Id = x.Id,
            CreatedAtUtc = x.CreatedAtUtc,
            UpdatedAtUtc = x.UpdatedAtUtc,
            Name = x.Name,
            StableId = x.StableId,
            State = x.State,
            Version = x.Version,
        });

        var _result = new ProcessSegmentListResponseDTO
        {
            Data = dtos,
        };

        return Ok(_result);
    }

    [ProducesResponseType(typeof(ProcessSegmentResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetProcessSegmentByIdQuery(id);

        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        var _result = new ProcessSegmentResponseDTO
        {
            Id = result.Value.Id,
            CreatedAtUtc = result.Value.CreatedAtUtc,
            UpdatedAtUtc = result.Value.UpdatedAtUtc,
            Name = result.Value.Name,
            StableId = result.Value.StableId,
            State = result.Value.State,
            Version = result.Value.Version,
            Parameters = result.Value.Parameters?.Select(p => new ProcessSegmentParameterResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Value = p.Value,
                Description = p.Description,
                DataType = p.DataType,
                CreatedAtUtc = p.CreatedAtUtc,
                UpdatedAtUtc = p.UpdatedAtUtc,
                ProcessSegmentId = result.Value.Id,
                IsReadOnly = p.IsReadOnly
            }).ToList(),
        };

        return Ok(_result);
    }

    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProcessSegmentCreateRequestDTO request, CancellationToken cancellationToken)
    {
        var command = new CreateProcessSegmentCommand(request.Name);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        return CreatedAtAction(nameof(Get), new { id = result.Value }, result.Value);
    }

    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpPost("{id:guid}/drafts")]
    public async Task<IActionResult> CreateDraft([FromRoute] Guid id, [FromBody] ProcessSegmentDraftCreateRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.ProcessSegmentId)
        {
            return BadRequest("The ID from the Route does not match the body");
        }


        var command = new CreateProcessSegmentDraftCommand(request.ProcessSegmentId);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        return CreatedAtAction(nameof(Get), new { id = result.Value }, result.Value);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpPatch("{id:guid}/release")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProcessSegmentReleaseRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Id in route does not match Id in body.");
        }

        var command = new ReleaseProcessSegmentCommand(id);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpPost("{id:guid}/parameters")]
    public async Task<IActionResult> CreateParameter([FromRoute] Guid id, [FromBody] ProcessSegmentParameterCreateRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.ProcessSegmentId)
        {
            return BadRequest("ID in route does not match ProcessSegmentId in body.");
        }

        var command = new CreateProcessSegmentParameterCommand(
            id,
            request.Name,
            request.Value,
            request.DataType,
            request.Description,
            request.IsReadOnly);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProcessSegmentUpdateRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.ProcessSegmentId)
        {
            return BadRequest("Id in route does not match Id in body.");
        }

        var command = new UpdateProcessSegmentCommand(
            id,
            request.Name);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpPut("{id:guid}/parameters/{parameterId:guid}")]
    public async Task<IActionResult> UpdateParameter([FromRoute] Guid id, [FromRoute] Guid parameterId, [FromBody] ProcessSegmentParameterUpdateRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.ProcessSegmentId)
        {
            return BadRequest("ID in route does not match ProcessSegmentId in body.");
        }

        if (parameterId != request.ParameterId)
        {
            return BadRequest("ParameterId in route does not match ParameterId in body.");
        }

        var command = new UpdateProcessSegmentParameterCommand(
            id,
            request.ParameterId,
            request.Name,
            request.Value,
            request.DataType,
            request.Description,
            request.IsReadOnly);


        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("The id cannot be empty");
        }


        var query = new DeleteProcessSegmentCommand(id);

        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpDelete("{id:guid}/parameters/{parameterId:guid}")]
    public async Task<IActionResult> UpdateProperty([FromRoute] Guid id, [FromRoute] Guid parameterId, CancellationToken cancellationToken)
    {

        var command = new DeleteProcessSegmentParameterCommand(
            id,
            parameterId);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        return NoContent();
    }
}
