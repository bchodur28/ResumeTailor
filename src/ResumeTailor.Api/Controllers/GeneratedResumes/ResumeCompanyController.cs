using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Models;

namespace ResumeTailor.Api.Controllers.GeneratedResumes;

[ApiController]
[Route("api/resumes")]
public sealed class ResumeCompanyController(IGeneratedResumeManagementService managementService) : ControllerBase
{
    [HttpPost("{generatedResumeId:int}/companies")]
    public async Task<ActionResult> CreateCompanySelectionsAsync(int generatedResumeId, [FromBody] IEnumerable<ResumeCompanySelectionRequest> requests, CancellationToken cancellationToken = default)
    {
        await managementService.CreateCompanySelectionsAsync(generatedResumeId, requests, cancellationToken);
        return NoContent();
    }

    [HttpPut("companies/{id:int}")]
    public async Task<ActionResult> UpdateCompanySelectionAsync(int id, [FromBody] ResumeCompanySelectionRequest request, CancellationToken cancellationToken = default)
    {
        await managementService.UpdateCompanySelectionAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("companies/{id:int}")]
    public async Task<ActionResult> DeleteCompanySelectionAsync(int id, CancellationToken cancellationToken = default)
    {
        await managementService.DeleteCompanySelectionAsync(id, cancellationToken);
        return NoContent();
    }
}
