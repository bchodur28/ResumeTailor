using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.Extraction.Models;

namespace ResumeTailor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FieldPatternController(IFieldPatternService service) : ControllerBase
{
    [HttpGet("{id:int}", Name = "GetFieldPatternById")]
    public async Task<ActionResult<FieldPatternResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await service.GetByIdAsync(id, cancellationToken);

        return Ok(response);
    }

    [HttpGet("field/{fieldId:int}")]
    public async Task<ActionResult<IReadOnlyCollection<FieldPatternResponse>>> GetByFieldIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await service.GetByFieldIdAsync(id, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<FieldPatternResponse>> CreateAsync(FieldPatternRequest request, CancellationToken cancellationToken = default)
    {
        var response = await service.CreateAsync(request, cancellationToken);

        return CreatedAtRoute("GetFieldPatternById", new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, FieldPatternRequest request, CancellationToken cancellationToken = default)
    {
        await service.UpdateAsync(id, request, cancellationToken);

        return NoContent();
    }
}
