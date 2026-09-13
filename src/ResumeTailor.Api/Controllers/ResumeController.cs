using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Api.Models;
using ResumeTailor.Application.GeneratedResumes.Common;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Models;

namespace ResumeTailor.Api.Controllers;

[ApiController]
[Route("api/resumes")]
public class ResumeController(IGeneratedResumeManagementService managementService, IResumeGeneratorService generatorService) : ControllerBase
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

    [HttpPost("companies/{resumeCompanyId:int}/bullets")]
    public async Task<ActionResult> CreateResumeBulletsAsync(int resumeCompanyId, [FromBody] IEnumerable<ResumeBulletRequest> requests, CancellationToken cancellationToken = default)
    {
        await managementService.CreateResumeBulletsAsycn(resumeCompanyId, requests, cancellationToken);
        return NoContent();
    }

    [HttpPut("bullets/{id:int}")]
    public async Task<ActionResult> UpdateResumeBulletAsync(int id, [FromBody] ResumeBulletRequest request, CancellationToken cancellationToken = default)
    {
        await managementService.UpdateResumeBulletAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("bullets/{id:int}")]
    public async Task<ActionResult> DeleteResumeBulletAsync(int id, CancellationToken cancellationToken = default)
    {
        await managementService.DeleteResumeBulletAsync(id, cancellationToken);
        return  NoContent();
    }
}
