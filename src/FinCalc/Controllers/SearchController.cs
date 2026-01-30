using System.ComponentModel.DataAnnotations;
using FinCalc.DataStructures;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces("application/json")]
public class SearchController : ControllerBase
{
	[HttpGet("{api}/search")]
	[ProducesResponseType(typeof(Asset[]), StatusCodes.Status200OK)]
	public async Task<ActionResult<Asset[]>> GetSecuritiesList([FromQuery] string query, [FromRoute] [AllowedValues("moex", "yfinance")] string api)
	{
		IRemoteAPI API = IRemoteAPI.FromString(api);
		Asset[] assets = await API.SecuritiesList(query);
		return Ok(assets);
	}
}