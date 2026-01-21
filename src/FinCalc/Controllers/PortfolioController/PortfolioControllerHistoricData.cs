using FinCalc.DataStructures;
using FinCalc.MOEXAPI;

using Microsoft.AspNetCore.Mvc;

public partial class PortfolioController : ControllerBase
{
	[HttpGet]
	[Route("totalHistoricValues")]
	[ProducesResponseType(typeof(HistoricData), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricData>> GetPortfolioValue([FromQuery] bool update, [FromQuery] int freq = 7, [FromQuery] int length = 52)
	{
		string json = System.IO.File.ReadAllText("./stored_portfolio.json");
		Portfolio portfolio = Portfolio.Deserialize(json);

		//check pre-assign values
		if (portfolio.Assets.Length == 0) throw new Exception("Portfolio was not initialized properly. No assets are assigned");

		if (update) portfolio = await AssignPortfolioValues.Whole(portfolio, freq, length);

		portfolio.TotalHistoricValues = portfolio.GetTotalHistoricValues();

		System.IO.File.WriteAllText("./stored_portfolio.json", Portfolio.Serialize(portfolio));
		
		return Ok(portfolio.TotalHistoricValues);
	}

	[HttpGet]
	[Route("assetsHistoricPrices")]
	[ProducesResponseType(typeof(HistoricData[]), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricData[]>> GetAssetsHistoricPrices([FromQuery] bool update, [FromQuery] int freq = 7, [FromQuery] int length = 52)
	{
		string json = System.IO.File.ReadAllText("./stored_portfolio.json");
		Portfolio portfolio = Portfolio.Deserialize(json);

		//check pre-assign values
		if (portfolio.Assets.Length == 0) throw new Exception("Portfolio was not initialized properly. No assets are assigned");
		if (portfolio.AssetsHistoricPrices == null) throw new Exception("Portfolio was not initialized properly. AssetsHistoricPrices is unassigned");

		if (update)
		{
			portfolio = await AssignPortfolioValues.Whole(portfolio, freq, length);
			
			System.IO.File.WriteAllText("./stored_portfolio.json", Portfolio.Serialize(portfolio));
		}
		
		return Ok(portfolio.AssetsHistoricPrices);
	}
}