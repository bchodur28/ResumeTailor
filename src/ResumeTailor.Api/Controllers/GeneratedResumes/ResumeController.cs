using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Api.Models;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Models;

namespace ResumeTailor.Api.Controllers.GeneratedResumes;

[ApiController]
[Route("api/resumes")]
public sealed class ResumeController(IGeneratedResumeManagementService managementService, IResumeGeneratorService generatorService) : ControllerBase
{

    [HttpPost("{accountId:int}/generate")]
    public async Task<ActionResult<GeneratedResumeDetailsResponse>> GenerateResumeAsync(int account, [FromBody] GenerateResumeRequest request, CancellationToken cancellationToken = default)
    {
        var response = await generatorService.GenerateResumeReviewAsync(account, request.Description, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:int}", Name = "GetGeneratedResume")]
    public async Task<ActionResult<GeneratedResumeDetailsResponse>> GetGeneratedResumeAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await managementService.GetGeneratedResumeAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<int>> SaveGeneratedResumeAsync([FromBody] SaveGeneratedResumeRequest request, CancellationToken cancellationToken = default)
    {
        var id = await managementService.SaveGeneratedResumeAsync(request, cancellationToken);
        return CreatedAtRoute("GetGeneratedResume", new { id }, id);
    }

    [HttpGet("account/{accountId:int}")]
    public async Task<ActionResult<IReadOnlyCollection<GeneratedResumeListItemResponse>>> GetGeneratedResumesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        var response = await managementService.GetGeneratedResumesByAccountIdAsync(accountId, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateGeneratedResumeAsync(int id, [FromBody] GeneratedResumeRequest request, CancellationToken cancellationToken = default)
    {
        await managementService.UpdateGeneratedResumeAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteGeneratedResumeAsync(int id, CancellationToken cancellationToken = default)
    {
        await managementService.DeleteGeneratedResumeAsync(id, cancellationToken);
        return NoContent();
    }
}
