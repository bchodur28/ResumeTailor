using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Contracts.Accounts;
using ResumeTailor.Application.Profile.Accounts.Interfaces;

namespace ResumeTailor.Api.Controllers.Profile
{

    [Route("api/accounts")]
    public class AccountController(IAccountService accountService) : AuthenticatedControllerBase
    {
        [HttpGet("me")]
        public async Task<ActionResult<AccountResponse>> GetCurrentAccountAsync(CancellationToken cancellationToken = default)
        {
            var auth0UserId = GetAuth0UserId();

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
            var auth0UserId = GetAuth0UserId();

            var id = await accountService.CreateAccountAsync(auth0UserId, request, cancellationToken);
            return CreatedAtRoute("GetAccountByAuth0UserId", new { auth0UserId }, id);
        }

        [HttpPut("me")]
        public async Task<ActionResult> UpdateAccountAsync([FromBody] AccountRequest request, CancellationToken cancellationToken = default)
        {
            var auth0UserId = GetAuth0UserId();

            await accountService.UpdateAccountAsync(auth0UserId, request, cancellationToken);
            return NoContent();
        }
    }
}
