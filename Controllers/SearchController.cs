using FinCalc.DataStructures;
using FinCalc.MOEXAPI;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("search")]
[Produces("application/json")]
[Consumes("application/json")]                        
public class SearchController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(Asset[]), StatusCodes.Status200OK)]
    public async Task<ActionResult<Asset[]>> GET([FromQuery] string partialName, [FromQuery] string market)
    {
        Asset[] assets = await GetFromMOEXAPI.SecuritiesList(partialName, market);
        return Ok(assets);
    }
}