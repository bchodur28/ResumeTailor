using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Api.Models;
using ResumeTailor.Application.Resumes.Interfaces;
using ResumeTailor.Application.Resumes.Models;

namespace ResumeTailor.Api.Controllers;

[ApiController]
[Route("api/resumes")]
public class ResumeController(IResumeManagementService managementService, IResumeGeneratorService generatorService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ResumeResponse>> GetResumeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await managementService.GetResumeByIdAsync(id, cancellationToken);

        return Ok(response);
    }

    [HttpPost("{id:int}/generate")]
    public async Task<ActionResult<CompanyGeneratedResume>> GenerateResumeAsync(int id, [FromBody] GenerateResumeRequest request, CancellationToken cancellationToken = default)
    {
        var response = await generatorService.GenerateResumeReviewAsync(id, request.Description, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> CreateResumeAsync(ResumeRequest request, CancellationToken cancellationToken = default)
    {
        await managementService.CreateResumeAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpPost("company")]
    public async Task<ActionResult> CreateCompanyAsync(CompanyRequest request, CancellationToken cancellationToken = default)
    {
        await managementService.CreateCompanyAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpPost("bullet")]
    public async Task<ActionResult> CreateBulletAsync(BulletRequest request, CancellationToken cancellationToken = default)
    {
        await managementService.CreateBulletAsync(request, cancellationToken);
        return NoContent();
    }
}
