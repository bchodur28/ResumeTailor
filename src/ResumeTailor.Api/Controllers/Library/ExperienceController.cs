using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Contracts.Experiences;
using ResumeTailor.Application.Library.Interfaces;
using System.Security.Claims;


namespace ResumeTailor.Api.Controllers.Library;

[Authorize]
[ApiController]
[Route("api/experiences")]
public class ExperienceController(ILibraryExperienceService experienceService) : ControllerBase
{
    [HttpGet("summaries")]
    public async Task<ActionResult<IReadOnlyCollection<LibraryExperienceSummaryResponse>>> GetSummariesAsync(CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        var summaries = await experienceService.GetExperienceSummariesAsync(auth0UserId, cancellationToken);
        return Ok(summaries);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LibraryExperienceResponse>> GetExperienceAsync(int id, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        var experience = await experienceService.GetExperienceAsync(auth0UserId, id, cancellationToken);
        return Ok(experience);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateExperienceAsync(LibraryExperienceRequest request, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        var experienceId = await experienceService.CreateExperienceAsync(auth0UserId, request, cancellationToken);
        return Ok(experienceId);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateExperienceAsync(int id, LibraryExperienceRequest request, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        await experienceService.UpdateExperienceAsync(auth0UserId, id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteExperienceAsync(int id, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        await experienceService.DeleteExperienceAsync(auth0UserId, id, cancellationToken);
        return NoContent();
    }

    private string GetAuth0UserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException(
                "Authenticated user is missing the required identifier claim.");
    }
}
