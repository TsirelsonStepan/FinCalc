using Microsoft.AspNetCore.Mvc;
using FinCalc.DataStructures;

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
}