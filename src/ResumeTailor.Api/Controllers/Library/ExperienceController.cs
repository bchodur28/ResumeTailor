using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Contracts.Experiences;
using ResumeTailor.Application.Library.Interfaces;
using System.Security.Claims;


namespace ResumeTailor.Api.Controllers.Library;

[Authorize]
[ApiController]
[Route("api/experiences")]
public class ExperienceController(ILibraryExperienceService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<LibraryExperienceResponse>>> GetExperiencesAsync(CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        var experiences = await service.GetExperiencesAsync(auth0UserId, cancellationToken);
        return Ok(experiences);
    }

    [HttpGet("list-items")]
    public async Task<ActionResult<IReadOnlyCollection<LibraryExperienceListItemResponse>>> GetExperienceListItemsAsync([FromQuery] bool archived = false, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        var listItems = await service.GetExperienceListItemsAsync(auth0UserId, archived, cancellationToken);
        return Ok(listItems);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LibraryExperienceResponse>> GetExperienceAsync(int id, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        var experience = await service.GetExperienceAsync(auth0UserId, id, cancellationToken);
        return Ok(experience);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateExperienceAsync(LibraryExperienceRequest request, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        var experienceId = await service.CreateExperienceAsync(auth0UserId, request, cancellationToken);
        return Ok(experienceId);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateExperienceAsync(int id, LibraryExperienceRequest request, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        await service.UpdateExperienceAsync(auth0UserId, id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteExperienceAsync(int id, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        await service.DeleteExperienceAsync(auth0UserId, id, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int}/archive")]
    public async Task<IActionResult> ArchiveExperienceAsync(int id, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        await service.ArchiveExperienceAsync(auth0UserId, id, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int}/restore")]
    public async Task<IActionResult> RestoreExperienceAsync(int id, CancellationToken cancellationToken = default)
    {
        var auth0UserId = GetAuth0UserId();

        await service.RestoreExperienceAsync(auth0UserId, id, cancellationToken);
        return NoContent();
    }

    private string GetAuth0UserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException(
                "Authenticated user is missing the required identifier claim.");
    }
}
