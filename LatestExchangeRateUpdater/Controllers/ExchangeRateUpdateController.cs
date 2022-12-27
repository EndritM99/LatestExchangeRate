using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Controllers
{
    public class ExchangeRateUpdateController : Controller
    {
        private readonly IExchangeRateUpdate _exchangeRateUpdate;

        public ExchangeRateUpdateController(IExchangeRateUpdate exchangeRateUpdate)
        {
            _exchangeRateUpdate = exchangeRateUpdate;
        }

        [HttpGet]
        [Route("updatelatestexchange")]
        public ActionResult<LatestExchangeRateResponse> UpdateLatestExchange(LatestExchangeRateRequest latestExchangeRateRequest)
        {
            var result = _exchangeRateUpdate.ExchangeRateUpdateServiceAsync(latestExchangeRateRequest);

            return Ok(result);
        }
    }
}
