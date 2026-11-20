using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Rendering.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Domain.Resumes.ApplicationTracking;

namespace ResumeTailor.Api.Controllers.GeneratedResumes;

[Route("api/resumes")]
public sealed class ResumeController(
    IResumeManagementService managementService,
    IResumeGenerator generatorService,
    IResumePdfGenerator pdfGenerator) : AuthenticatedControllerBase
{

    [HttpPost("{accountId:int}/generate")]
    public async Task<ActionResult<int>> GenerateResumeAsync([FromBody] GeneratedResumeRequest request, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        var response = await generatorService.GenerateResumeAsync(auth0UserId, request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:int}/pdf")]
    public async Task<ActionResult> GetResumePdf(int id, CancellationToken cancellationToken)
    {
        var auth0UserId = GetAuth0UserId();

        var resumeDetails = await managementService.GetResumeDetailsAsync(auth0UserId, id, cancellationToken);
        var pdf = pdfGenerator.Generate(resumeDetails.Resume);

        return File(pdf, "application/pdf", $"{resumeDetails.Resume.ResumeName}");
    }

    [HttpGet("{id:int}", Name = "GetGeneratedResume")]
    public async Task<ActionResult<ResumeDetailsResponse>> GetResumeAsync(int id, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        var response = await managementService.GetResumeDetailsAsync(auth0UserId, id, cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ResumeExperienceSummaryResponse>>> GetResumeSummariesByAccountIdAsync([FromQuery] ResumeSummaryQuery request, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        var response = await managementService.GetPagedResumeSummariesByAccountIdAsync(auth0UserId, request, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ResumeDetailsResponse>> UpdateResumeAsync(int id, [FromBody] UpdateResumeRequest request, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        await managementService.UpdateResumeAsync(auth0UserId, id, request, cancellationToken);

        var response = await managementService.GetResumeDetailsAsync(auth0UserId, id, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteResumeAsync(int id, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        await managementService.DeleteResumeDetailsAsync(auth0UserId, id, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/job-posting")]
    public async Task<ActionResult> UpdateResumeJobPostingAsync(int id, [FromBody] ResumeJobPostingRequest request, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        await managementService.UpdateResumeJobPostingAsync(auth0UserId, id, request, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}/application-tracking")]
    public async Task<ActionResult> UpdateResumeApplicationTrackingAsync(int id, [FromBody] ApplicationStatus request, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        await managementService.UpdateResumeApplicationTrackingAsync(auth0UserId, id, request, cancellationToken);
        return NoContent();
    }
}
