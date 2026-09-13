using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Api.Models;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;

namespace ResumeTailor.Api.Controllers;

[ApiController]
[Route("api/resumes")]
public class ResumeController(IGeneratedResumeManagementService managementService, IResumeGeneratorService generatorService) : ControllerBase
{

    [HttpPost("{id:int}/generate")]
    public async Task<ActionResult<GeneratedResumeDetailsResponse>> GenerateResumeAsync(int id, [FromBody] GenerateResumeRequest request, CancellationToken cancellationToken = default)
    {
        var response = await generatorService.GenerateResumeReviewAsync(id, request.Description, cancellationToken);

        return Ok(response);
    }

    
}
