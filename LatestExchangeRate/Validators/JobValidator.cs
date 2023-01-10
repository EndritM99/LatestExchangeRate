using Hangfire.States;
using Hangfire;
using LatestExchangeRate.Models;

namespace LatestExchangeRate.Validators
{
    public class JobValidator
    {
        public bool DoesJobHasBeenEnqueued(string jobId)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                return false;
            }

            try
            {
                var connection = JobStorage.Current.GetConnection();
                var stateData = connection.GetStateData(jobId);
                return stateData.Name == EnqueuedState.StateName;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}
