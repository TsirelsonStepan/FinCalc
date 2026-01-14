using Microsoft.AspNetCore.Mvc;
using FinCalc.DataStructures;

public partial class PortfolioController : ControllerBase
{
	[HttpGet]
	[Route("totalHistoricValues")]
	[ProducesResponseType(typeof(HistoricData), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricData>> GetPortfolioValue([FromQuery] bool update, [FromQuery] int freq = 7, [FromQuery] int period = 52)
	{
		string json = System.IO.File.ReadAllText("./stored_portfolio.json");
		Portfolio portfolio = Portfolio.Deserialize(json);

		if (update || portfolio.TotalHistoricValues == null)
		{
			portfolio.TotalHistoricValues = await portfolio.GetTotalHistoricValues(freq, period);
		}
		return Ok(portfolio.TotalHistoricValues);
	}

	[HttpGet]
	[Route("assetsHistoricPrices")]
	[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
	public async Task<ActionResult<double>> GetAssetsHistoricPrices([FromQuery] bool update, [FromQuery] int freq = 7, [FromQuery] int period = 52)
	{
		string json = System.IO.File.ReadAllText("./stored_portfolio.json");
		Portfolio portfolio = Portfolio.Deserialize(json);

		if (update || portfolio.AssetsHistoricPrices.Length == 0)
		{
			portfolio.AssetsHistoricPrices = await portfolio.GetAssetsHistoricPrices(freq, period);
		}
		return Ok(portfolio.AssetsHistoricPrices);
	}
}