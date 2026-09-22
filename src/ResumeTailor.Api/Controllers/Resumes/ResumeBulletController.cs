using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Contracts.Bullets;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;

namespace ResumeTailor.Api.Controllers.GeneratedResumes;

[ApiController]
[Route("api/resumes")]
public sealed class ResumeBulletController(IResumeManagementService managementService) : ControllerBase
{
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
        return NoContent();
    }
}
