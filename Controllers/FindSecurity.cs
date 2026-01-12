using FinCalc.DataStructures;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("getSecuritiesList")]
[Produces("application/json")]
[Consumes("application/json")]
public class GetSecuritiesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(Asset[]), StatusCodes.Status200OK)]
    public async Task<ActionResult<Asset[]>> FindSecurity([FromQuery] string partialName)
    {
        Asset[] assets = await FinCalc.MOEXAPI.Get.SecuritiesList(partialName);
        return Ok(assets);
    }
}