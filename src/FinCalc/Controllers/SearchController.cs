using FinCalc.DataStructures;
using FinCalc.RemoteAPIs;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces("application/json")]
public class SearchController : ControllerBase
{
	private readonly IRemoteAPI API = new MOEXAPI();

	[HttpGet("search")]
	[ProducesResponseType(typeof(Asset[]), StatusCodes.Status200OK)]
	public async Task<ActionResult<Asset[]>> GetSecuritiesList([FromQuery] string partialName, [FromQuery] string source)
	{
		Asset[] assets = await API.SecuritiesList(partialName, source);
		return Ok(assets);
	}
}