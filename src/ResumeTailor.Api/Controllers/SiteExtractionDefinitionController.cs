namespace ResumeTailor.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.Extraction.Models;

[ApiController]
[Route("api/[controller]")]
public class SiteExtractionDefinitionController(ISiteExtractionDefinitionService service) : ControllerBase
{
    [HttpGet("{id:int}", Name = "GetSiteExtractionDefinitionById")]
    public async Task<ActionResult<SiteExtractionDefinitionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await service.GetByIdAsync(id, cancellationToken);

        return Ok(response);
    }

    [HttpGet("enabled")]
    public async Task<ActionResult<IReadOnlyCollection<SiteExtractionDefinitionResponse>>> GetEnabledAsync(CancellationToken cancellationToken = default)
    {
        var response = await service.GetEnabledAsync(cancellationToken);

        return Ok(response);
    }

    [HttpGet("match")]
    public async Task<ActionResult<SiteExtractionDefinitionResponse>> GetMatchingAsync(string hostname, string path, CancellationToken cancellationToken = default)
    {
        var response = await service.GetMatchingDefinitionAsync(hostname, path, cancellationToken);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<SiteExtractionDefinitionResponse>> CreateAsync(SiteExtractionDefinitionRequest request, CancellationToken cancellationToken = default)
    {
        var response = await service.CreateAsync(request, cancellationToken);

        return CreatedAtRoute("GetSiteExtractionDefinitionById", new { id= response.Id }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, SiteExtractionDefinitionRequest request, CancellationToken cancellationToken = default)
    {
        await service.UpdateAsync(id, request, cancellationToken);

        return NoContent();
    }

}
