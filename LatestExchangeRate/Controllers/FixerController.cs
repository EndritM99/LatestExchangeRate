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
        public void LatestExchangeRate(FixerRestClientRequest fixerRestClientRequest)
        {
            if (fixerRestClientRequest == null)
            {
                throw new Exception("Null Req");
            }

            _jobScheduler.EnqueueGetLatestExchangeRate(fixerRestClientRequest);
        }
    }
}
