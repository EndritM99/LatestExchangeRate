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
                return BadRequest("Request was null, please try again!");
            }

            try
            {
                var jobId = _jobScheduler.EnqueueGetLatestExchangeRate(fixerRestClientRequest);

                if (string.IsNullOrEmpty(jobId))
                {
                    return BadRequest("Error scheduling job: job ID was null or empty");
                }

                var connection = JobStorage.Current.GetConnection();
                var stateData = connection.GetStateData(jobId);
                var operationResponse = new OperationResponse();

                if (stateData.Name == EnqueuedState.StateName)
                {
                    // The job was successfully scheduled
                    operationResponse.Success = true;
                    _jobScheduler.EnqueueWriteResponseToFile();

                    var writeJobId = _jobScheduler.EnqueueWriteResponseToFile();

                    if (string.IsNullOrEmpty(writeJobId))
                    {
                        return BadRequest("Error scheduling write job: job ID was null or empty");
                    }

                    var writeStateData = connection.GetStateData(writeJobId);

                    if (writeStateData.Name == EnqueuedState.StateName)
                    {
                        // The write job was successfully scheduled
                        operationResponse.Success = true;
                        return Ok(operationResponse);
                    }
                    else
                    {
                        // The write job was not successfully scheduled
                        operationResponse.Success = false;
                        return BadRequest(operationResponse);
                    }
                }
                else
                {
                    // The job was not successfully scheduled
                    operationResponse.Success = false;
                    return BadRequest(operationResponse);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error scheduling job: {ex.Message}");
            }
        }

    }
}
