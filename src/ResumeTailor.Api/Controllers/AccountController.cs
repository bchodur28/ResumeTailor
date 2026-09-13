using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.Accounts.Interfaces;
using ResumeTailor.Application.Accounts.Models;

namespace ResumeTailor.Api.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        [HttpGet("me")]
        public async Task<ActionResult<AccountResponse>> GetCurrentAccountAsync(CancellationToken cancellationToken = default)
        {
            var auth0UserId = User.FindFirst("sub")?.Value;

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

        [HttpGet("{auth0UserId:string}", Name = "GetAccountByAuth0UserId")]
        public async Task<ActionResult<AccountResponse?>> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default)
        {
            var account = await accountService.GetAccountByAuth0UserIdAsync(auth0UserId, cancellationToken);
            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccountAsync([FromBody] AccountRequest request, CancellationToken cancellationToken = default)
        {
            var id = await accountService.CreateAccountAsync(request, cancellationToken);
            return CreatedAtRoute("GetAccountByAuth0UserId", new { auth0UserId = request.Auth0UserId }, id);
        }

        [HttpPost("{accountId:int}/personal-links")]
        public async Task<ActionResult> CreatePersonalLinksAsync(int accountId, [FromBody] IEnumerable<PersonalLinkRequest> requests, CancellationToken cancellationToken = default)
        {
            await accountService.CreatePersonalLinksAsync(accountId, requests, cancellationToken);
            return NoContent();
        }

        [HttpPut("personal-links/{id:int}")]
        public async Task<ActionResult> UpdatePersonalLinkAsync(int id, [FromBody] PersonalLinkRequest request, CancellationToken cancellationToken = default)
        {
            await accountService.UpdatePersonalLinkAsync(id, request, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{accountId:int}/personal-links")]
        public async Task<ActionResult> DeletePersonalLinksAsync(int accountId, [FromBody] IEnumerable<int> personalLinkIds, CancellationToken cancellationToken = default)
        {
            await accountService.DeletePersonalLinksAsync(accountId, personalLinkIds, cancellationToken);
            return NoContent();
        }

        [HttpPost("{accountId:int}/titles")]
        public async Task<ActionResult> CreateTitlessAsync(int accountId, [FromBody] IEnumerable<TitleRequest> requests, CancellationToken cancellationToken = default)
        {
            await accountService.CreateTitlesAsync(accountId, requests, cancellationToken);
            return NoContent();
        }

        [HttpPut("titles/{id:int}")]
        public async Task<ActionResult> UpdateTitleAsync(int id, [FromBody] TitleRequest request, CancellationToken cancellationToken = default)
        {
            await accountService.UpdateTitleAsync(id, request, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{accountId:int}/titles")]
        public async Task<ActionResult> DeleteTitlesAsync(int accountId, [FromBody] IEnumerable<int> titleIds, CancellationToken cancellationToken = default)
        {
            await accountService.DeleteTitlesAsync(accountId, titleIds, cancellationToken);
            return NoContent();
        }
    }
}
