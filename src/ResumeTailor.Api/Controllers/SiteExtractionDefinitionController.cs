namespace ResumeTailor.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Extraction;
using ResumeTailor.Application.Extraction.Models;

[ApiController]
[Route("api/[controller]")]
public class SiteExtractionDefinitionController(ISiteExtractionDefinitionService service) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SiteExtractionDefinitionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var siteExtractionResponse = await service.GetByIdAsync(id, cancellationToken);

        return Ok(siteExtractionResponse);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<SiteExtractionDefinitionResponse>>> GetEnabledAsync(CancellationToken cancellationToken = default)
    {
        var siteExtractionListResponse = await service.GetEnabledAsync(cancellationToken);

        return Ok(siteExtractionListResponse);
    }

    [HttpGet("match")]
    public async Task<ActionResult<SiteExtractionDefinitionResponse>> GetMatchingAsync(string hostname, string path, CancellationToken cancellationToken = default)
    {
        var siteExtractionResponse = await service.GetMatchingDefinitionAsync(hostname, path, cancellationToken);

        if (siteExtractionResponse is null)
        {
            return NotFound();
        }

        return Ok(siteExtractionResponse);
    }

    [HttpPost]
    public async Task<ActionResult<SiteExtractionDefinitionResponse>> CreateAsync(SiteExtractionDefinitionRequest request, CancellationToken cancellationToken = default)
    {
        var siteExtractionresponse = await service.CreateAsync(request, cancellationToken);
        return Ok(siteExtractionresponse);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, SiteExtractionDefinitionRequest request, CancellationToken cancellationToken = default)
    {
        await service.UpdateAsync(id, request, cancellationToken);

        return Ok();
    }

}
