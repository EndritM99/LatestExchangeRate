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
        public async Task<ActionResult<OperationResponse>> UpdateLatestExchange(LatestExchangeRateRequest latestExchangeRateRequest)
        { 
            //Since the request can be null I'm not validating anything before calling the service :D
            var response = await _exchangeRateUpdate.ExchangeRateUpdateServiceAsync(latestExchangeRateRequest);

            if (response.Success == true)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
