using Hangfire;
using Hangfire.States;
using LatestExchangeRate.Interfaces;
using LatestExchangeRate.Models;
using Microsoft.AspNetCore.Mvc;

namespace LatestExchangeRate.Controllers
{
    public class FixerController : Controller
    {
        private readonly IJobScheduler _jobScheduler;

        public FixerController(IJobScheduler jobScheduler)
        {
            _jobScheduler = jobScheduler;
        }

        [HttpGet]
        [Route("latestexchangerate")]
        public ActionResult<OperationResponse> LatestExchangeRate(FixerRestClientRequest fixerRestClientRequest)
        {
            if (fixerRestClientRequest == null)
            {
                throw new Exception("Request was null, please try again!");
            }

            var jobId = _jobScheduler.EnqueueGetLatestExchangeRate(fixerRestClientRequest);

            var connection = JobStorage.Current.GetConnection();
            var stateData = connection.GetStateData(jobId);
            var operationResponse = new OperationResponse();

            if (stateData.Name == EnqueuedState.StateName)
            {
                // The job was successfully scheduled
                operationResponse.Success = true;
                return Ok(operationResponse);
            }
            else
            {
                // The job was not successfully scheduled
                operationResponse.Success = false;
                return BadRequest(operationResponse);
            }
        }
    }
}
