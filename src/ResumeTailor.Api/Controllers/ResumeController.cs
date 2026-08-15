using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Resumes.Interfaces;
using ResumeTailor.Application.Resumes.Models;

namespace ResumeTailor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResumeController(IResumeService service) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ResumeResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await service.GetResumeByIdAsync(id, cancellationToken);

        return Ok(response);
    }
}
