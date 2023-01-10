﻿using Hangfire;
using Hangfire.States;
using LatestExchangeRate.Interfaces;
using LatestExchangeRate.Models;
using LatestExchangeRate.Validators;
using Microsoft.AspNetCore.Mvc;

namespace LatestExchangeRate.Controllers
{
    public class FixerController : Controller
    {
        private readonly IJobScheduler _jobScheduler;
        private readonly JobValidator _jobValidator;

        public FixerController(IJobScheduler jobScheduler, JobValidator jobValidator)
        {
            _jobScheduler = jobScheduler;
            _jobValidator = jobValidator;
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

                var operationResponse = new OperationResponse();

                if (_jobValidator.DoesJobHasBeenEnqueued(jobId))
                {
                    // The job was successfully scheduled
                    operationResponse.Success = true;

                    var writeJobId = _jobScheduler.EnqueueWriteResponseToFile();

                    if (_jobValidator.DoesJobHasBeenEnqueued(writeJobId))
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
