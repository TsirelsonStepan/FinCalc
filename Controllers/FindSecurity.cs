using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("getSecuritiesList")]
public class GetSecuritiesController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> FindSecurity([FromQuery] string query)
    {
        return Ok(await FinCalc.MOEXAPI.Get.SecuritiesList(query));
    }
}