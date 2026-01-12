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
    public async Task<IActionResult> CalculatePortfolio([FromBody] AssetInPortfolio[] assets)
    {
        Portfolio portfolio = await Portfolio.CreateAsync(assets);
        
        return Ok(portfolio);
    }
}