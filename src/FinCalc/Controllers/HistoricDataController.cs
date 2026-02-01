using Microsoft.AspNetCore.Mvc;

using FinCalc.Calculate;
using FinCalc.Models;
using FinCalc.Models.DTOs;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("historic")]
public class HistoricDataController : ControllerBase
{
	[HttpPost("prices")]
	[ProducesResponseType(typeof(HistoricDataResponse), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricDataResponse>> GetAssetPrices([FromBody] HistoricDataRequest request)
	{
		CustomContext context = new();
		HistoricData raw = await IRemoteAPI.FromString(request.Source.Api).Prices(
			context,
			request.Source.AssetPath,
			request.TimeSeries!.Frequency!.Value,
			request.TimeSeries.Period!.Value);

		HistoricData result = Historic.FitDates(raw);
		return Ok(new { data = new HistoricDataResponse(result), notes = context.GetNotes() });
	}
/*
	[HttpPost("returns")]
	[ProducesResponseType(typeof(HistoricDataResponse), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricDataResponse>> GetAssetReturns([FromBody] HistoricDataRequest request)
	{
		CustomContext context = new();
		HistoricData prices = await IRemoteAPI.FromString(request.Source.Api).Prices(
			context,
			request.Source.AssetPath,
			request.TimeSeries!.Frequency!.Value,
			request.TimeSeries.Period!.Value);
		HistoricData returns = Historic.Returns(prices);

		return Ok(new { data = new HistoricDataResponse(returns), notes = context.GetNotes()});
	}
*/
}