using Hangfire;
using LatestExchangeRate.Interfaces;
using LatestExchangeRate.Models;

namespace LatestExchangeRate.Services
{
    public class JobScheduler : IJobScheduler
    {
        private readonly IExchangeRate _fixerService;

        public JobScheduler(IExchangeRate fixerService)
        {
            _fixerService = fixerService;
        }

        public string EnqueueGetLatestExchangeRate(FixerRestClientRequest request)
        {
            var jobId = BackgroundJob.Enqueue(() => _fixerService.GetLatestExchangeRate(request));

            return jobId;
        }
    }
}
