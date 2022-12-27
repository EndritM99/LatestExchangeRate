using ExchangeRateDataProvider.Interfaces;
using ExchangeRateDataProvider.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateDataProvider.Controllers
{
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRateDataRepository _repository;

        public ExchangeRateController(IExchangeRateDataRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("getlatestexchangerate")]
        public ActionResult<FixerDataResponse> GetLatestExchangeRate()
        {
            var response = _repository.GetLatestExchangeRate();
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
