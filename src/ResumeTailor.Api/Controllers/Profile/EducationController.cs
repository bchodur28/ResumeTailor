using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Profile.Education.Interfaces;


namespace ResumeTailor.Api.Controllers.Profile
{
    
    [Route("api/education")]
    public class EducationController(IEducationService educationService) : AuthenticatedControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<LibraryEducationResponse>>> GetEducationListAsync([FromQuery] bool archived, CancellationToken cancellationToken = default)
        {
            var auth0UserId = GetAuth0UserId();

            var response = await educationService.GetListAsync(auth0UserId, archived, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateEducationAsync(EducationRequest request, CancellationToken cancellationToken = default)
        {
            var auth0UserId = GetAuth0UserId();

            await educationService.CreateAsync(auth0UserId, request, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateEducationAsync(int id, EducationRequest request, CancellationToken cancellationToken = default)
        {
            var auth0UserId = GetAuth0UserId();

            await educationService.UpdateAsync(auth0UserId, id, request, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEducationAsync(int id, CancellationToken cancellationToken = default)
        {
            var auth0UserId = GetAuth0UserId();

            await educationService.DeleteAsync(auth0UserId, id, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id:int}/archive")]
        public async Task<IActionResult> ArchiveExperienceAsync(int id, CancellationToken cancellationToken = default)
        {
            var auth0UserId = GetAuth0UserId();

            await educationService.ArchiveAsync(auth0UserId, id, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id:int}/restore")]
        public async Task<IActionResult> RestoreExperienceAsync(int id, CancellationToken cancellationToken = default)
        {
            var auth0UserId = GetAuth0UserId();

            await educationService.RestoreAsync(auth0UserId, id, cancellationToken);
            return NoContent();
        }
    }
}
