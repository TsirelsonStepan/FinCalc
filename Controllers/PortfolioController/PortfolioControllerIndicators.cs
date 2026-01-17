/*
using FinCalc.DataStructures;
using FinCalc.Calculator;
using FinCalc.MOEXAPI;

using Microsoft.AspNetCore.Mvc;

public partial class PortfolioController : ControllerBase
{
	[HttpGet]
	[Route("WAPR")]
	[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
	public async Task<ActionResult<double>> GetWARP([FromQuery] bool update, [FromQuery] int freq, [FromQuery] int length)
	{
		string json = System.IO.File.ReadAllText("./stored_portfolio.json");
		Portfolio portfolio = Portfolio.Deserialize(json);

		//check pre-assign values
		if (portfolio.Benchmark == null) throw new Exception("Portfolio was not initialized properly. No benchmark is assigned");
		if (portfolio.Assets.Length == 0) throw new Exception("Portfolio was not initialized properly. No assets are assigned");

		if (update)
		{
			portfolio = await AssignPortfolioValues.Whole(portfolio, freq, length);

	        portfolio.WeightedAverageReturn = portfolio.GetWeightedAverageReturn();

			System.IO.File.WriteAllText("./stored_portfolio.json", Portfolio.Serialize(portfolio));
		}
		else if (portfolio.WeightedAverageReturn == null) throw new Exception("Portfolio was not initialized properly. WeightedAverageReturn is unassigned");

		return Ok(portfolio.WeightedAverageReturn);
	}

	[HttpGet]
	[Route("EPR")]
	[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
	public async Task<ActionResult<double>> GetEPR([FromQuery] bool update, [FromQuery] int freq, [FromQuery] int length)
	{
		string json = System.IO.File.ReadAllText("./stored_portfolio.json");
		Portfolio portfolio = Portfolio.Deserialize(json);

		//check pre-assign values
		if (portfolio.Benchmark == null) throw new Exception("Portfolio was not initialized properly. No benchmark is assigned");
		if (portfolio.Assets.Length == 0) throw new Exception("Portfolio was not initialized properly. No assets are assigned");

		if (update)
		{
			portfolio = await AssignPortfolioValues.Whole(portfolio, freq, length);
			portfolio.Beta = portfolio.GetBeta();

			portfolio.ExpectedReturn = portfolio.GetCAPM(
				portfolio.RiskFreeRate ?? throw new Exception("Portfolio was not initialized properly. RiskFreeRate == null"),
				portfolio.Beta ?? throw new Exception("Portfolio was not initialized properly. Beta == null"),
				Calculate.AnnualizeReturns(Calculate.Returns(portfolio.HistoricBenchmarkPrices ?? throw new Exception("Portfolio was not initialized properly. HistoricBenchmarkPrices == null"))));

			System.IO.File.WriteAllText("./stored_portfolio.json", Portfolio.Serialize(portfolio));
		}
		else if (portfolio.ExpectedReturn == null) throw new Exception("Portfolio was not initialized properly. ExpectedReturn == null");

		return Ok(portfolio.ExpectedReturn);
	}
}
*/