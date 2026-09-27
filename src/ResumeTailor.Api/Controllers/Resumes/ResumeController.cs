using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Api.Models;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;

namespace ResumeTailor.Api.Controllers.GeneratedResumes;

[ApiController]
[Route("api/resumes")]
public sealed class ResumeController(IResumeManagementService managementService, IResumeGeneratorService generatorService) : ControllerBase
{

    [HttpPost("{accountId:int}/generate")]
    public async Task<ActionResult<int>> GenerateResumeAsync(int accountId, [FromBody] GenerateResumeRequest request, CancellationToken cancellationToken = default)
    {
        var response = await generatorService.GenerateResumeDetailsAsync(accountId, request.Description, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:int}", Name = "GetGeneratedResume")]
    public async Task<ActionResult<ResumeDetailsResponse>> GetResumeAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await managementService.GetResumeDetailsAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpGet("account/{accountId:int}")]
    public async Task<ActionResult<IReadOnlyCollection<ResumeListItemResponse>>> GetResumesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        var response = await managementService.GetResumesByAccountIdAsync(accountId, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateResumeAsync(int id, [FromBody] UpdateResumeRequest request, CancellationToken cancellationToken = default)
    {
        await managementService.UpdateResumeAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteResumeAsync(int id, CancellationToken cancellationToken = default)
    {
        await managementService.DeleteResumeDetailsAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/job-posting")]
    public async Task<ActionResult> UpdateResumeJobPostingAsync(int id, [FromBody] ResumeJobPostingRequest request, CancellationToken cancellationToken = default)
    {
        await managementService.UpdateResumeJobPostingAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/application-tracking")]
    public async Task<ActionResult> UpdateResumeApplicationTrackingAsync(int id, [FromBody] ResumeApplicationTrackingRequest request, CancellationToken cancellationToken = default)
    {
        await managementService.UpdateResumeApplicationTrackingAsync(id, request, cancellationToken);
        return NoContent();
    }
}
