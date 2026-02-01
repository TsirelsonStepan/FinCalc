using System.ComponentModel.DataAnnotations;

using FinCalc.Models;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces("application/json")]
public class SearchController : ControllerBase
{
	[HttpGet("{api}/search")]
	[ProducesResponseType(typeof(IReadOnlyList<Asset>), StatusCodes.Status200OK)]
	public async Task<ActionResult<IReadOnlyList<Asset>>> GetSecuritiesList([FromQuery] string query, [FromRoute] [AllowedValues("moex", "yfinance")] string api)
	{
		IRemoteAPI API = IRemoteAPI.FromString(api);
		return Ok(await API.SecuritiesList(query));
	}
}