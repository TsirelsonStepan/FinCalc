using FinCalc.DataStructures;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public class HistoricDataController : ControllerBase
{
	[HttpGet]
	[Route("historicAssetReturns")]
	[ProducesResponseType(typeof(HistoricData), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricData>> GetAssetReturns([FromQuery] string secid, [FromQuery] string market, [FromQuery] int period = 365)
	{
		HistoricData historicReturns = FinCalc.Calculate.BaseIndicator.Returns(await FinCalc.MOEXAPI.Get.Prices(market, secid, period));
		return Ok(historicReturns);
	}

	[HttpGet]
	[Route("historicAssetPrices")]
	[ProducesResponseType(typeof(HistoricData), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricData>> GetAssetPrices([FromQuery] string secid, [FromQuery] string market, [FromQuery] int period = 365)
	{
		HistoricData historicPrices = await FinCalc.MOEXAPI.Get.Prices(market, secid, period);
		return Ok(historicPrices);
	}

	[HttpGet]
	[Route("totalHistoricValues")]
	[ProducesResponseType(typeof(HistoricData), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricData>> GetPortfolioValue([FromQuery] int interval = 7, [FromQuery] int periods = 52)
	{
		string json = System.IO.File.ReadAllText("./stored_portfolio.json");
		Portfolio portfolio = Portfolio.Deserialize(json);
		HistoricData? historicValue = await portfolio.GetTotalHistoricValues(interval, periods);
		return Ok(historicValue);
	}

	[HttpGet]
	[Route("assetsHistoricPrices")]
	[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
	public async Task<ActionResult<double>> GetAssetsHistoricPrices([FromQuery] int interval = 7, [FromQuery] int periods = 52)
	{
		string json = System.IO.File.ReadAllText("./stored_portfolio.json");
		Portfolio portfolio = Portfolio.Deserialize(json);
		HistoricData[] historicPrices = await portfolio.GetAssetsHistoricPrices(interval, periods);
		return Ok(historicPrices);
	}
}