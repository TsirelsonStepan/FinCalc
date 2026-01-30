using FinCalc.DataStructures;
using FinCalc.Calculate;

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("historicData")]
public class HistoricDataController : ControllerBase
{
	[HttpPost("prices")]
	[ProducesResponseType(typeof(HistoricDataResponce), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricDataResponce>> GetAssetPrices([FromBody] HistoricDataRequest request)
	{
		CustomContext context = new();
		HistoricData historicPrices = await IRemoteAPI.FromString(request.Source.Api).Prices(
			context,
			request.Source.AssetPath,
			request.TimeSeries!.Frequency!.Value,
			request.TimeSeries.Period!.Value);

		return Ok(new { data = new HistoricDataResponce(historicPrices), notes = context.GetNotes() });
	}

	[HttpPost("returns")]
	[ProducesResponseType(typeof(HistoricDataResponce), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricDataResponce>> GetAssetReturns([FromBody] HistoricDataRequest request)
	{
		CustomContext context = new();
		HistoricData prices = await IRemoteAPI.FromString(request.Source.Api).Prices(
			context,
			request.Source.AssetPath,
			request.TimeSeries!.Frequency!.Value,
			request.TimeSeries.Period!.Value);
		HistoricData returns = Historic.Returns(prices);

		return Ok(new { data = new HistoricDataResponce(returns), notes = context.GetNotes()});
	}

	[HttpPost("portfolio/values")]
	[ProducesResponseType(typeof(HistoricDataResponce), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricDataResponce>> GetPortfolioValue([FromBody] AssetInPortfolio[] assets, [FromQuery] TimeSeriesRequest timeSeries)
	{
		CustomContext context = new();

		HistoricData[] assetPrices = await GetMultiplePrices(context, assets, timeSeries);
		double[] amounts = new double[assets.Length];
		for (int i = 0; i < assets.Length; i++) amounts[i] = assets[i].Amount;
		HistoricData total = Historic.Total(context, assetPrices, amounts);

		return Ok(new { data = new HistoricDataResponce(total), notes = context.GetNotes() });
	}

	[HttpPost("portfolio/prices")]
	[ProducesResponseType(typeof(HistoricDataResponce[]), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricDataResponce[]>> GetAssetsHistoricPrices([FromBody] AssetInPortfolio[] assets, [FromQuery] TimeSeriesRequest timeSeries)
	{
		CustomContext context = new();
		HistoricData[] assetPrices = await GetMultiplePrices(context, assets, timeSeries);
		HistoricDataResponce[] result = new HistoricDataResponce[assets.Length];
		for (int i = 0; i < assets.Length; i++) result[i] = new HistoricDataResponce(assetPrices[i]);

		return Ok(new { data = result, notes = context.GetNotes() });
	}

	private async Task<HistoricData[]> GetMultiplePrices(CustomContext context, AssetInPortfolio[] assets, TimeSeriesRequest timeSeries)
	{
		HistoricData[] result = new HistoricData[assets.Length];
		for (int i = 0; i < assets.Length; i++)
		{
			HistoricData prices = await IRemoteAPI.FromString(assets[i].Source.Api).Prices(
				context,
				assets[i].Source.AssetPath,
				timeSeries.Frequency!.Value,
				timeSeries.Period!.Value);
			result[i] = prices;
		}
		return result;
	}
}