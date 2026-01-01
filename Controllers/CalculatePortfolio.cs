using Microsoft.AspNetCore.Mvc;
using FinCalc.DataStructures;

[ApiController]
[Route("calculatePortfolio")]
public class CalculatePortfolioController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CalculatePortfolio([FromBody] Asset[] assets)
    {
        Portfolio portfolio = new(assets);
        await FinCalc.MOEXAPI.Get.RFRate();

        portfolio.Verify();
        await portfolio.CalcualteWeightedAverageReturn();
        await portfolio.CalculateBeta();
        await portfolio.CalculateExpectedReturn();
        return Ok(portfolio);
    }
}