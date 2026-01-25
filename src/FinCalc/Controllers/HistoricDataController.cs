using FinCalc.DataStructures;
using FinCalc.Calculate;
using FinCalc.RemoteAPIs;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/historicData")]
public class HistoricDataController : ControllerBase
{
	private readonly IRemoteAPI API = new MOEXAPI();

	[HttpPost("prices")]
	[ProducesResponseType(typeof(HistoricDataResponce), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricDataResponce>> GetAssetPrices([FromBody] HistoricDataRequest request)
	{
		HistoricData historicPrices = await API.Prices(request.Source!.Market!, request.Secid!, request.TimeSeries!.Frequency!.Value, request.TimeSeries.Period!.Value);

		return Ok(new HistoricDataResponce(historicPrices));
	}

	[HttpPost("returns")]
	[ProducesResponseType(typeof(HistoricDataResponce), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricDataResponce>> GetAssetReturns([FromBody] HistoricDataRequest request)
	{
		HistoricData historicReturns = Historic.Returns(await API.Prices(request.Source!.Market!, request.Secid!, request.TimeSeries!.Frequency!.Value, request.TimeSeries.Period!.Value));

		return Ok(new HistoricDataResponce(historicReturns));
	}

	[HttpPost("portfolio/values")]
	[ProducesResponseType(typeof(HistoricDataResponce), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricDataResponce>> GetPortfolioValue([FromBody] AssetInPortfolio[] assets, [FromQuery] TimeSeriesRequest timeSeries)
	{
		CustomContext context = new();

		HistoricData[] assetPrices = await Helper(assets, timeSeries);
		double[] amounts = new double[assets.Length];
		for (int i = 0; i < assets.Length; i++) amounts[i] = assets[i].Amount;
		HistoricData total = Historic.Total(context, assetPrices, amounts);

		return Ok(new { data = new HistoricDataResponce(total), notes = context.GetNotes() });
	}

	[HttpPost("portfolio/prices")]
	[ProducesResponseType(typeof(HistoricDataResponce[]), StatusCodes.Status200OK)]
	public async Task<ActionResult<HistoricDataResponce[]>> GetAssetsHistoricPrices([FromBody] AssetInPortfolio[] assets, [FromQuery] TimeSeriesRequest timeSeries)
	{
		HistoricData[] assetPrices = await Helper(assets, timeSeries);
		HistoricDataResponce[] result = new HistoricDataResponce[assets.Length];
		for (int i = 0; i < assets.Length; i++) result[i] = new HistoricDataResponce(assetPrices[i]);

		return Ok(result);
	}

	private async Task<HistoricData[]> Helper(AssetInPortfolio[] assets, TimeSeriesRequest timeSeries)
	{
		HistoricData[] result  = new HistoricData[assets.Length];
		for (int i = 0; i < assets.Length; i++)
		{
			HistoricData prices = await API.Prices(assets[i].Market!, assets[i].Secid!, timeSeries.Frequency!.Value, timeSeries.Period!.Value);
			result[i] = prices;
		}
		return result;
	}
}