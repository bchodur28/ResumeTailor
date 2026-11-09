using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ResumeTailor.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class AuthenticatedControllerBase : ControllerBase
    {
        protected string GetAuth0UserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("Authenticated user is missing the required identifier claim.");
        }
    }
}
