using Microsoft.AspNetCore.Mvc;
using FinCalc.DataStructures;

[ApiController]
[Route("calculatePortfolio")]
[Produces("application/json")]
[Consumes("application/json")]
public class CalculatePortfolioController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(Portfolio), StatusCodes.Status200OK)]
    public async Task<IActionResult> CalculatePortfolio([FromBody] Asset[] assets)
    {
        Portfolio portfolio = new(assets);

        portfolio.Verify();
        await FinCalc.MOEXAPI.Get.RFRate();
        await portfolio.CalculatePriceHistory();
        await portfolio.CalcualteWeightedAverageReturn();
        await portfolio.CalculateBeta();
        await portfolio.CalculateExpectedReturn();
        return Ok(portfolio);
    }
}