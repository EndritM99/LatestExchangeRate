using Hangfire;
using LatestExchangeRate.Interfaces;
using LatestExchangeRate.Models;

namespace LatestExchangeRate.Services
{
    public class JobScheduler : IJobScheduler
    {
        private readonly IExchangeRate _fixerService;
        private readonly IDocumentProcessing _documentProcessingService;

        public JobScheduler(IExchangeRate fixerService, IDocumentProcessing documentProcessing)
        {
            _fixerService = fixerService;
            _documentProcessingService = documentProcessing;
        }

        public string EnqueueGetLatestExchangeRate(FixerRestClientRequest request)
        {
            var jobId = BackgroundJob.Enqueue(() => _fixerService.GetLatestExchangeRate(request));

            return jobId;
        }

        public string EnqueueWriteResponseToFile()
        {
            var jobId = BackgroundJob.Enqueue(() => _documentProcessingService.WriteResponseToFile());
            return jobId;
        }
    }
}
