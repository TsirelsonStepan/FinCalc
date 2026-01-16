using FinCalc.Calculator;
using FinCalc.DataStructures;
using FinCalc.MOEXAPI;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public partial class PortfolioController : ControllerBase
{
	[HttpPost]
	[Route("portfolio")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult> POST([FromBody] AssetInPortfolio[] assets)
	{
		int freq = 7;
		int period = 52 * 5;

		Portfolio portfolio = new(assets, new AssetInPortfolio("index", "IMOEX", 1));
		portfolio = await AssignPortfolioValues.Whole(portfolio, freq, period);

		portfolio.TotalHistoricValues = portfolio.GetTotalHistoricValues();
		portfolio.Beta = portfolio.GetBeta();
		portfolio.WeightedAverageReturn = portfolio.GetWeightedAverageReturn();
		portfolio.ExpectedReturn = portfolio.GetCAPM(
			portfolio.RiskFreeRate ?? throw new Exception("Portfolio was not initialized properly. RiskFreeRate == null"),
			portfolio.Beta ?? throw new Exception("Portfolio was not initialized properly. Beta == null"),
			Calculate.AnnualizeReturns(Calculate.Returns(portfolio.HistoricBenchmarkPrices ?? throw new Exception("Portfolio was not initialized properly. HistoricBenchmarkPrices == null")))
		);

		System.IO.File.WriteAllText("./stored_portfolio.json", Portfolio.Serialize(portfolio));

		return Ok();
	}

	[HttpGet]
	[Route("portfolio")]
	[ProducesResponseType(typeof(Portfolio), StatusCodes.Status200OK)]
	public ActionResult<Portfolio> GET()
	{
		Portfolio portfolio = Portfolio.Deserialize(System.IO.File.ReadAllText("./stored_portfolio.json"));

		return Ok(portfolio);
	}
}