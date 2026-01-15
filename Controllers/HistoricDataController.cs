using FinCalc.DataStructures;
using FinCalc.Calculator;
using FinCalc.MOEXAPI;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public class HistoricDataController : ControllerBase
{
	[HttpGet]
	[Route("historicAssetPrices")]
	[ProducesResponseType(typeof(HistoricData), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricData>> GetAssetPrices([FromQuery] string secid, [FromQuery] string market, [FromQuery] int freq = 7, [FromQuery] int period = 52)
	{
		HistoricData historicPrices = await GetFromMOEXAPI.Prices(market, secid, freq, period);
		return Ok(historicPrices);
	}

	[HttpGet]
	[Route("historicAssetReturns")]
	[ProducesResponseType(typeof(HistoricData), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricData>> GetAssetReturns([FromQuery] string market, [FromQuery] string secid, [FromQuery] int freq = 7, [FromQuery] int period = 52)
	{
		HistoricData historicReturns = Calculate.Returns(await GetFromMOEXAPI.Prices(market, secid, freq, period));
		return Ok(historicReturns);
	}
}