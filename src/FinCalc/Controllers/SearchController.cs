using FinCalc.DataStructures;
using FinCalc.MOEXAPI;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces("application/json")]
public class SearchController : ControllerBase
{
	[HttpGet("search")]
	[ProducesResponseType(typeof(Asset[]), StatusCodes.Status200OK)]
	public async Task<ActionResult<Asset[]>> GetSecuritiesList([FromQuery] string partialName, [FromQuery] string market)
	{
		Asset[] assets = await GetFromMOEXAPI.SecuritiesList(partialName, market);
		return Ok(assets);
	}
}