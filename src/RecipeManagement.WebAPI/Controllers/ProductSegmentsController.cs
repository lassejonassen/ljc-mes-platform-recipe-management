using Microsoft.AspNetCore.Mvc;
using RecipeManagement.Application.ProcessSegments.Commands;
using RecipeManagement.Application.ProductSegments.Commands;
using RecipeManagement.Application.ProductSegments.Queries;
using RecipeManagement.WebAPI.Contracts.ProcessSegments;
using RecipeManagement.WebAPI.Contracts.ProductSegments;

namespace RecipeManagement.WebAPI.Controllers;


[Route("api/product-segments")]
public class ProductSegmentsController : BaseController
{
    [ProducesResponseType(typeof(ProductSegmentListResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllProductSegmentsQuery();

        var result = await Mediator.Send(query, cancellationToken);

        if (!result.Any())
        {
            return NoContent();
        }

        var dtos = result.Select(x => new ProductSegmentResponseDTO
        {
            Id = x.Id,
            MaterialDefinitionId = x.MaterialDefinitionId,
            MaterialName = x.MaterialName,
            MaterialSku = x.MaterialSku,
            ProcessSegmentId = x.ProcessSegmentId,
            ProcessSegmentName = x.ProcessSegmentName,
            State = x.State,
            Version = x.Version
        });

        var _result = new ProductSegmentListResponseDTO
        {
            Data = dtos,
        };

        return Ok(_result);
    }

    [ProducesResponseType(typeof(ProductSegmentResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetProductSegmentByIdQuery(id);

        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        var _result = new ProductSegmentResponseDTO
        {
            Id = result.Value.Id,
            MaterialDefinitionId = result.Value.MaterialDefinitionId,
            MaterialName = result.Value.MaterialName,
            MaterialSku = result.Value.MaterialSku,
            ProcessSegmentId = result.Value.ProcessSegmentId,
            ProcessSegmentName = result.Value.ProcessSegmentName,
            State = result.Value.State,
            Version = result.Value.Version,
            Parameters = result.Value.Parameters?.Select(p => new ProductSegmentParameterResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Value = p.Value,
                Description = p.Description,
                DataType = p.DataType,
            }).ToList(),
        };

        return Ok(_result);
    }

    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductSegmentCreateRequestDTO request, CancellationToken cancellationToken)
    {
        var command = new CreateProductSegmentCommand(request.MaterialDefinitionId, request.ProcessSegmentId);

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
    public async Task<IActionResult> CreateDraft([FromRoute] Guid id, [FromBody] ProductSegmentDraftCreateRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.ProductSegmentId)
        {
            return BadRequest("The ID from the Route does not match the body");
        }

        var command = new CreateProductSegmentDraftCommand(request.ProductSegmentId);

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
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProductSegmentReleaseRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.ProductSegmentId)
        {
            return BadRequest("Id in route does not match Id in body.");
        }

        var command = new ReleaseProductSegmentCommand(id);

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
    public async Task<IActionResult> UpdateParameter([FromRoute] Guid id, [FromRoute] Guid parameterId, [FromBody] ProductSegmentParameterUpdateRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.ProductSegmentId)
        {
            return BadRequest("ID in route does not match ProductSegmentId in body.");
        }

        if (parameterId != request.ProductSegmentParameterId)
        {
            return BadRequest("ParameterId in route does not match ParameterId in body.");
        }

        var command = new UpdateProductSegmentParameterCommand(
            id, parameterId, request.Value
            );


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


        var query = new DeleteProductSegmentCommand(id);

        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        return NoContent();
    }
}
