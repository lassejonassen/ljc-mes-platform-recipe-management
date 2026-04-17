using Microsoft.AspNetCore.Mvc;
using RecipeManagement.Application.MaterialDefinitions.Commands;
using RecipeManagement.Application.MaterialDefinitions.Queries;
using RecipeManagement.WebAPI.Contracts.MaterialDefinitions;

namespace RecipeManagement.WebAPI.Controllers;

[Route("api/material-definitions")]
public class MaterialDefinitionsController : BaseController
{
    [ProducesResponseType(typeof(MaterialDefinitionListResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllMaterialDefinitionsQuery();

        var result = await Mediator.Send(query, cancellationToken);

        if (!result.Any())
        {
            return NoContent();
        }

        var dtos = result.Select(x => new MaterialDefinitionResponseDTO
        {
            Id = x.Id,
            Name = x.Name,
            Sku = x.Sku,
            CreatedAtUtc = x.CreatedAtUtc,
            UpdatedAtUtc = x.UpdatedAtUtc,
        });

        var _result = new MaterialDefinitionListResponseDTO
        {
            Data = dtos,
        };

        return Ok(_result);
    }

    [ProducesResponseType(typeof(MaterialDefinitionResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetMaterialDefinitionByIdQuery(id);

        var result = await Mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        var _result = new MaterialDefinitionResponseDTO
        {
            Id = result.Value.Id,
            Sku = result.Value.Sku,
            Name = result.Value.Name,
            CreatedAtUtc = result.Value.CreatedAtUtc,
            UpdatedAtUtc = result.Value.UpdatedAtUtc,
            Properties = result.Value.Properties?.Select(p => new MaterialDefinitionPropertyResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Value = p.Value,
                Description = p.Description,
                DataType = p.DataType,
                CreatedAtUtc = p.CreatedAtUtc,
                UpdatedAtUtc = p.UpdatedAtUtc,
            }).ToList(),
        };

        return Ok(_result);
    }

    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MaterialDefinitionCreateRequestDTO request, CancellationToken cancellationToken)
    {
        var command = new CreateMaterialDefinitionCommand(request.Sku, request.Name);

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

    [HttpPost("{id:guid}/properties")]
    public async Task<IActionResult> CreateProperty([FromRoute] Guid id, [FromBody] MaterialDefinitionPropertyCreateRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.MaterialDefinitionId)
        {
            return BadRequest("MaterialDefinitionId in route does not match MaterialDefinitionId in body.");
        }

        var command = new CreateMaterialDefinitionPropertyCommand(
            id,
            request.Name,
            request.Value,
            request.DataType,
            request.Description
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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] MaterialDefinitionUpdateRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
        {
            return BadRequest("Id in route does not match Id in body.");
        }

        var command = new UpdateMaterialDefinitionCommand(
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

    [HttpPut("{id:guid}/properties/{propertyId:guid}")]
    public async Task<IActionResult> UpdateProperty([FromRoute] Guid id,[FromRoute] Guid propertyId, [FromBody] MaterialDefinitionPropertyUpdateRequestDTO request, CancellationToken cancellationToken)
    {
        if (id != request.MaterialDefinitionId)
        {
            return BadRequest("MaterialDefinitionId in route does not match MaterialDefinitionId in body.");
        }

        if (propertyId != request.PropertyId)
        {
            return BadRequest("PropertyId in route does not match PropertyId in body.");
        }

        var command = new UpdateMaterialDefinitionPropertyCommand(
            id,
            propertyId,
            request.Name,
            request.Value,
            request.DataType,
            request.Description
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


        var query = new DeleteMaterialDefinitionCommand(id);

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

    [HttpDelete("{id:guid}/properties/{propertyId:guid}")]
    public async Task<IActionResult> UpdateProperty([FromRoute] Guid id, [FromRoute] Guid propertyId, CancellationToken cancellationToken)
    {

        var command = new DeleteMaterialDefinitionPropertyCommand(
            id,
            propertyId);

        var result = await Mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result.Error);
        }

        return NoContent();
    }
}
