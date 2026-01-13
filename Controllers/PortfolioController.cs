using Microsoft.AspNetCore.Mvc;
using FinCalc.DataStructures;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public class PortfolioController : ControllerBase
{
	[HttpPost]
	[Route("portfolio")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult> POST([FromBody] AssetInPortfolio[] assets)
	{
		Portfolio portfolio = await Portfolio.CreateAsync(assets);

		System.IO.File.WriteAllText("./stored_portfolio.json", Portfolio.Serialize(portfolio));

		return Ok();
	}

	[HttpGet]
	[Route("portfolio")]
	[ProducesResponseType(typeof(Portfolio), StatusCodes.Status200OK)]
	public async Task<ActionResult<Portfolio>> GET()
	{
		Portfolio portfolio = Portfolio.Deserialize(System.IO.File.ReadAllText("./stored_portfolio.json"));

		return Ok(portfolio);
	}

	[HttpGet]
	[Route("WAPR")]
	[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
	public async Task<ActionResult<double>> GetWARP()
	{
		string json = System.IO.File.ReadAllText("./stored_portfolio.json");
		Portfolio portfolio = Portfolio.Deserialize(json);

		return Ok(portfolio.WeightedAveragePortfolioReturn);
	}

	[HttpGet]
	[Route("EPR")]
	[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
	public async Task<ActionResult<double>> GetEPR()
	{
		string json = System.IO.File.ReadAllText("./stored_portfolio.json");
		Portfolio portfolio = Portfolio.Deserialize(json);

		return Ok(portfolio.ExpectedPortfolioReturn);
	}
}