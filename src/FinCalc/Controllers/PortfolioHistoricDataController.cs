using Microsoft.AspNetCore.Mvc;

using FinCalc.Calculate;
using FinCalc.Models;
using FinCalc.Models.DTOs;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("portfolio")]
public class PortfolioHistoricDataController : ControllerBase
{
	[HttpPost("values")]
	[ProducesResponseType(typeof(HistoricDataResponse), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricDataResponse>> GetPortfolioValue([FromBody] IReadOnlyList<AssetInPortfolio> assets, [FromQuery] TimeSeriesRequest timeSeries)
	{
		CustomContext context = new();

		IReadOnlyList<HistoricData> assetPrices = await GetMultiplePrices(context, assets, timeSeries);
		double[] amounts = new double[assets.Count];
		for (int i = 0; i < assets.Count; i++) amounts[i] = assets[i].Amount;
		HistoricData total = Historic.Total(context, assetPrices, amounts);

		return Ok(new { data = new HistoricDataResponse(total), notes = context.GetNotes() });
	}
/*
	[HttpPost("prices")]
	[ProducesResponseType(typeof(IReadOnlyList<HistoricDataResponse>), StatusCodes.Status200OK)]
	public async Task<ActionResult<IReadOnlyList<HistoricDataResponse>>> GetAssetsHistoricPrices([FromBody] IReadOnlyList<AssetInPortfolio> assets, [FromQuery] TimeSeriesRequest timeSeries)
	{
		CustomContext context = new();
		IReadOnlyList<HistoricData> assetPrices = await GetMultiplePrices(context, assets, timeSeries);
		HistoricDataResponse[] result = new HistoricDataResponse[assets.Count];
		for (int i = 0; i < assets.Count; i++) result[i] = new HistoricDataResponse(Historic.FitDates(assetPrices[i]));

		return Ok(new { data = result, notes = context.GetNotes() });
	}
*/
	private async Task<IReadOnlyList<HistoricData>> GetMultiplePrices(CustomContext context, IReadOnlyList<AssetInPortfolio> assets, TimeSeriesRequest timeSeries)
	{
		List<HistoricData> result = [];
		for (int i = 0; i < assets.Count; i++)
		{
			HistoricData prices = await IRemoteAPI.FromString(assets[i].Source.Api).Prices(
				context,
				assets[i].Source.AssetPath,
				timeSeries.Frequency!.Value,
				timeSeries.Period!.Value);
			if (prices.Dates.Count == 0) continue;
			result.Add(prices);
		}
		return result;
	}
}