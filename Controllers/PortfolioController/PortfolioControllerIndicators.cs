using FinCalc.DataStructures;
using FinCalc.Calculator;
using FinCalc.MOEXAPI;

using Microsoft.AspNetCore.Mvc;

public partial class PortfolioController : ControllerBase
{
	[HttpGet]
	[Route("WAPR")]
	[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
	public async Task<ActionResult<double>> GetWARP([FromQuery] bool update)
	{
		string json = System.IO.File.ReadAllText("./stored_portfolio.json");
		Portfolio portfolio = Portfolio.Deserialize(json);

		if (update || portfolio.WeightedAverageReturn == null)
		{
	        portfolio.WeightedAverageReturn = await portfolio.GetWeightedAverageReturn();
		}

		return Ok(portfolio.WeightedAverageReturn);
	}

	[HttpGet]
	[Route("EPR")]
	[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
	public async Task<ActionResult<double>> GetEPR([FromQuery] bool update)
	{
		string json = System.IO.File.ReadAllText("./stored_portfolio.json");
		Portfolio portfolio = Portfolio.Deserialize(json);

		if (update || portfolio.ExpectedReturn == null)
		{
			double rfrate = await GetFromMOEXAPI.RFRate();
			double PortfolioBeta = await portfolio.GetBeta();
			portfolio.ExpectedReturn = Calculate.CAPM(await GetFromMOEXAPI.Prices("index", "IMOEX", 90, 40), PortfolioBeta, rfrate);
		}

		return Ok(portfolio.ExpectedReturn);
	}
}