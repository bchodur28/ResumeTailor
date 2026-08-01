using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.Extraction.Models;

namespace ResumeTailor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FieldExtractionDefinitionController(IFieldExtractionDefinitionService service) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FieldExtractionDefinitionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var response = await service.GetByIdAsync(id, cancellationToken);

            return Ok(response);
        }

        [HttpGet("site/{siteId:int}")]
        public async Task<ActionResult<IReadOnlyCollection<FieldExtractionDefinitionResponse>>> GetBySiteIdAsync(int siteId, CancellationToken cancellationToken = default)
        {
            var response = service.GetBySiteIdAsync(siteId, cancellationToken);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<FieldExtractionDefinitionResponse>> CreateAsync(FieldExtractionDefinitionRequest request, CancellationToken cancellationToken = default)
        {
            var response = await service.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = response.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAsync(int id, FieldExtractionDefinitionRequest request, CancellationToken cancellationToken = default)
        {
            await service.UpdateAsync(id, request, cancellationToken);

            return NoContent();
        }
    }
}
