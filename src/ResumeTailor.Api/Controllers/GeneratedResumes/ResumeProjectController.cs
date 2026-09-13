using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Models;

namespace ResumeTailor.Api.Controllers.GeneratedResumes;

[ApiController]
[Route("api/resumes")]
public sealed class ResumeProjectController(IGeneratedResumeManagementService managementService) : ControllerBase
{
    [HttpPost("{generatedResumeId:int}/projects")]
    public async Task<ActionResult> CreateProjectSelectionsAsync(int generatedResumeId, [FromBody] IEnumerable<ResumeProjectSelectionRequest> requests, CancellationToken cancellationToken = default)
    {
        await managementService.CreateProjectSelectionsAsync(generatedResumeId, requests, cancellationToken);
        return NoContent();
    }

    [HttpPut("projects/{id:int}")]
    public async Task<ActionResult> UpdateProjectSelectionAsync(int id, [FromBody] ResumeProjectSelectionRequest request, CancellationToken cancellationToken = default)
    {
        await managementService.UpdateProjectSelectionAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("projects/{id:int}")]
    public async Task<ActionResult> DeleteProjectSelectionAsync(int id, CancellationToken cancellationToken = default)
    {
        await managementService.DeleteProjectSelectionAsync(id, cancellationToken);
        return NoContent();
    }
}
