using Microsoft.AspNetCore.Mvc;
using ResumeTailor.Application.JobApplications.Interfaces;
using ResumeTailor.Application.JobApplications.Models;

namespace ResumeTailor.Api.Controllers
{
    [ApiController]
    [Route("api/job-applications")]
    public class JobApplicationController(IJobApplicationService jobService) : ControllerBase
    {
        [HttpGet("account/{accountId:int}")]
        public async Task<ActionResult<IReadOnlyCollection<JobApplicationListItemResponse>>> GetJobListingByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
        {
            var response = await jobService.GetJobApplicationListItemsByAccountIdAsync(accountId, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<JobApplicationResponse>> GetJobApplicationByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var response = await jobService.GetJobApplicationIdAsync(id, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateJobApplicationAsync(JobApplicationRequest request, CancellationToken cancellationToken = default)
        {
            await jobService.CreateJobApplicationAsync(request, cancellationToken);

            return Created();
        }
    }
}
