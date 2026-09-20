using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Application.Profile.Accounts.Models;
using System.Security.Claims;

namespace ResumeTailor.Api.Controllers.Profile
{
    [Authorize]
    [ApiController]
    [Route("api/accounts")]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        [HttpGet("me")]
        public async Task<ActionResult<AccountResponse>> GetCurrentAccountAsync(CancellationToken cancellationToken = default)
        {
            var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0UserId is null)
            {
                return Unauthorized();
            }

            var account = await accountService.GetAccountByAuth0UserIdAsync(
                auth0UserId,
                cancellationToken);

            if (account is null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpGet("{auth0UserId}", Name = "GetAccountByAuth0UserId")]
        public async Task<ActionResult<AccountResponse?>> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default)
        {
            var account = await accountService.GetAccountByAuth0UserIdAsync(auth0UserId, cancellationToken);
            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccountAsync([FromBody] AccountRequest request, CancellationToken cancellationToken = default)
        {
            var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(string.IsNullOrWhiteSpace(auth0UserId))
            {
                return Unauthorized();
            }

            var id = await accountService.CreateAccountAsync(auth0UserId, request, cancellationToken);
            return CreatedAtRoute("GetAccountByAuth0UserId", new { auth0UserId }, id);
        }

        [HttpPut("me")]
        public async Task<ActionResult> UpdateAccountAsync([FromBody] AccountRequest request, CancellationToken cancellationToken = default)
        {
            var auth0UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(auth0UserId))
            {
                return Unauthorized();
            }
            await accountService.UpdateAccountAsync(auth0UserId, request, cancellationToken);
            return NoContent();
        }
    }
}
