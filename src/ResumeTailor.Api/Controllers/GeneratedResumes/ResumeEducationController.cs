using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Models;

namespace ResumeTailor.Api.Controllers.GeneratedResumes;

[ApiController]
[Route("api/resumes")]
public sealed class ResumeEducationController(IGeneratedResumeManagementService managementService) : ControllerBase
{
    [HttpPost("{generatedResumeId:int}/educations")]
    public async Task<ActionResult> CreateEducationSelectionsAsync(int generatedResumeId, [FromBody] IEnumerable<ResumeEducationSelectionRequest> requests, CancellationToken cancellationToken = default)
    {
        await managementService.CreateEducationSelectionsAsync(generatedResumeId, requests, cancellationToken);
        return NoContent();
    }

    [HttpPut("educations/{id:int}")]
    public async Task<ActionResult> UpdateEducationSelectionAsync(int id, [FromBody] ResumeEducationSelectionRequest request, CancellationToken cancellationToken = default)
    {
        await managementService.UpdateEducationSelectionAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("educations/{id:int}")]
    public async Task<ActionResult> DeleteEducationSelectionAsync(int id, CancellationToken cancellationToken = default)
    {
        await managementService.DeleteEducationSelectionAsync(id, cancellationToken);
        return NoContent();
    }
}
