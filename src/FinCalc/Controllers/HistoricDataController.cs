using FinCalc.DataStructures;
using FinCalc.Calculate;
using FinCalc.MOEXAPI;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces("application/json")]
[Route("api/historicData")]
public class HistoricDataController : ControllerBase
{
	[HttpGet("prices")]
	[ProducesResponseType(typeof(HistoricData), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricData>> GetAssetPrices([FromQuery] string market, [FromQuery] string secid, [FromQuery] int frequency, [FromQuery] int period)
	{
		HistoricData historicPrices = await GetFromMOEXAPI.Prices(market, secid, frequency, period);

		return Ok(new HistoricDataResponce(historicPrices));
	}

	[HttpGet("returns")]
	[ProducesResponseType(typeof(Dictionary<string, double?>), StatusCodes.Status200OK)]
	public async Task<ActionResult<Dictionary<string, double?>>> GetAssetReturns([FromQuery] string market, [FromQuery] string secid, [FromQuery] int frequency, [FromQuery] int period)
	{
		HistoricData historicReturns = Historic.Returns(await GetFromMOEXAPI.Prices(market, secid, frequency, period));

		return Ok(new HistoricDataResponce(historicReturns));
	}
}