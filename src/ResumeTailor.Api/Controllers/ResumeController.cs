using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Resumes.Interfaces;
using ResumeTailor.Application.Resumes.Models;

namespace ResumeTailor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResumeController(IResumeService service) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ResumeResponse>> GetResumeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await service.GetResumeByIdAsync(id, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> CreateResumeAsync(ResumeRequest request, CancellationToken cancellationToken = default)
    {
        await service.CreateResumeAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpPost("company")]
    public async Task<ActionResult> CreateCompanyAsync(CompanyRequest request, CancellationToken cancellationToken = default)
    {
        await service.CreateCompanyAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpPost("bullet")]
    public async Task<ActionResult> CreateBulletAsync(BulletRequest request, CancellationToken cancellationToken = default)
    {
        await service.CreateBulletAsync(request, cancellationToken);
        return NoContent();
    }
}
