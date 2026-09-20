using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Profile.Education.Interfaces;
using ResumeTailor.Application.Profile.Education.Models;
using System.Security.Claims;

namespace ResumeTailor.Api.Controllers.Profile
{
    [Authorize]
    [ApiController]
    [Route("api/education")]
    public class EducationController(IEducationService service) : ControllerBase
    {
        [HttpGet("me")]
        public async Task<ActionResult<IReadOnlyCollection<EducationResponse>>> GetCurrentEducationAsync(CancellationToken cancellationToken = default)
        {
            var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (auth0UserId == null)
            {
                return Unauthorized();
            }

            var response = await service.GetEducationAsync(auth0UserId, cancellationToken);

            return Ok(response);
        }

        [HttpPost("me")]
        public async Task<ActionResult> CreateEducationAsync([FromBody] IReadOnlyCollection<EducationRequest> requests, CancellationToken cancellationToken = default)
        {
            var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (auth0UserId == null)
            {
                return Unauthorized();
            }

            await service.CreateEducationAsync(auth0UserId, requests, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("me")]
        public async Task<ActionResult> UpdateEducationAsync([FromBody] IReadOnlyCollection<EducationRequest> requests, CancellationToken cancellationToken = default)
        {
            var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (auth0UserId == null)
            {
                return Unauthorized();
            }

            await service.UpdateEducatonAsync(auth0UserId, requests, cancellationToken);
            return NoContent();
        }

        [HttpDelete("me")]
        public async Task<ActionResult> DeleteEducationAsync([FromBody] IReadOnlyCollection<int> educationIds, CancellationToken cancellationToken = default)
        {
            var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (auth0UserId == null)
            {
                return Unauthorized();
            }

            await service.DeleteEducationAsync(auth0UserId, educationIds, cancellationToken);
            return NoContent();
        }
    }
}
