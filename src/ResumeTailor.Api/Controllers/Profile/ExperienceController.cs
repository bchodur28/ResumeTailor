using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Contracts.Bullets;
using ResumeTailor.Application.Contracts.Companies;
using ResumeTailor.Application.Contracts.Projects;
using ResumeTailor.Application.Profile.Experience.Interfaces;
using ResumeTailor.Application.Profile.Experience.Models;
using System.Security.Claims;

namespace ResumeTailor.Api.Controllers.Profile;

[Authorize]
[ApiController]
[Route("api/experience")]
public class ExperienceController(IExperenceService service) : ControllerBase
{
    [HttpGet("me/companies")]
    public async Task<ActionResult<IReadOnlyCollection<CompanyResponse>>> GetCompaniesAsync(CancellationToken cancellationToken)
    {
        var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0UserId is null)
        {
            return Unauthorized();
        }
        var companiesWithBullets = await service.GetCompaniesAsync(auth0UserId, cancellationToken);
        return Ok(companiesWithBullets);
    }

    [HttpPost("me/companies")]
    public async Task<ActionResult> CreateCompaniesAsync([FromBody] IReadOnlyCollection<CompanyRequest> requests, CancellationToken cancellationToken)
    {
        var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0UserId == null)
        {
            return Unauthorized();
        }

        await service.CreateCompaniesAsync(auth0UserId, requests, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("me/companies")]
    public async Task<ActionResult> UpdateCompaniesAsync([FromBody] IReadOnlyCollection<CompanyRequest> requests, CancellationToken cancellationToken)
    {
        var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0UserId == null)
        {
            return Unauthorized();
        }
        await service.UpdateCompaniesAsync(auth0UserId, requests, cancellationToken);
        return NoContent();
    }

    [HttpDelete("me/companies")]
    public async Task<ActionResult> DeleteCompaniesAsync([FromBody] IReadOnlyCollection<int> companyIds, CancellationToken cancellationToken)
    {
        var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0UserId == null)
        {
            return Unauthorized();
        }
        await service.DeleteCompaniesAsync(auth0UserId, companyIds, cancellationToken);
        return NoContent();
    }

    [HttpGet("me/projects")]
    public async Task<ActionResult<IReadOnlyCollection<ProjectResponse>>> GetProjectsAsync(CancellationToken cancellationToken)
    {
        var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0UserId is null)
        {
            return Unauthorized();
        }
        var companiesWithBullets = await service.GetProjectsAsync(auth0UserId, cancellationToken);
        return Ok(companiesWithBullets);
    }

    [HttpPost("me/projects")]
    public async Task<ActionResult> CreateProjectsAsync([FromBody] IReadOnlyCollection<ProjectRequest> requests, CancellationToken cancellationToken)
    {
        var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0UserId == null)
        {
            return Unauthorized();
        }

        await service.CreateProjectsAsync(auth0UserId, requests, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("me/projects")]
    public async Task<IActionResult> UpdateProjectsAsync([FromBody] IReadOnlyCollection<ProjectRequest> requests, CancellationToken cancellationToken)
    {
        var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0UserId == null)
        {
            return Unauthorized();
        }
        await service.UpdateProjectsAsync(auth0UserId, requests, cancellationToken);
        return NoContent();
    }

    [HttpDelete("me/projects")]
    public async Task<ActionResult> DeleteProjectsAsync([FromBody] IReadOnlyCollection<int> projectIds, CancellationToken cancellationToken)
    {
        var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0UserId == null)
        {
            return Unauthorized();
        }
        await service.DeleteProjectsAsync(auth0UserId, projectIds, cancellationToken);
        return NoContent();
    }

    [HttpGet("me/bullets")]
    public async Task<ActionResult<IReadOnlyCollection<CompanyBulletsResponse>>> GetCompanyBulletsAsync(CancellationToken cancellationToken)
    {
        var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0UserId is null)
        {
            return Unauthorized();
        }
        var companyBullets = await service.GetCompanyBulletsAsync(auth0UserId, cancellationToken);
        return Ok(companyBullets);
    }

    [HttpPost("me/bullets")]
    public async Task<ActionResult> CreateBulletsAsync([FromBody] IReadOnlyCollection<BulletRequest> requests, CancellationToken cancellationToken)
    {
        var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0UserId == null)
        {
            return Unauthorized();
        }

        await service.CreateBulletsAsync(auth0UserId, requests, cancellationToken);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("me/bullets")]
    public async Task<ActionResult> UpdateProjectsAsync([FromBody] IReadOnlyCollection<BulletRequest> requests, CancellationToken cancellationToken)
    {
        var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0UserId == null)
        {
            return Unauthorized();
        }
        await service.UpdateBulletsAsync(auth0UserId, requests, cancellationToken);
        return NoContent();
    }

    //Bullets
    [HttpDelete("me/bullets")]
    public async Task<ActionResult> DeleteProjectsAsync([FromBody] IReadOnlyCollection<BulletDeleteRequest> requests, CancellationToken cancellationToken)
    {
        var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (auth0UserId == null)
        {
            return Unauthorized();
        }
        await service.DeleteBulletsAsync(auth0UserId, requests, cancellationToken);
        return NoContent();
    }
}
