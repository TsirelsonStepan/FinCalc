using FinCalc.Calculate;
using FinCalc.DataStructures;
using FinCalc.MOEXAPI;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/portfolio/historicData")]
public partial class PortfolioController : ControllerBase
{
	[HttpPost("values")]
	[ProducesResponseType(typeof(Dictionary<string, double?>), StatusCodes.Status200OK)]
	public async Task<ActionResult<Dictionary<string, double?>>> GetPortfolioValue([FromBody] AssetInPortfolio[] assets, [FromQuery] TimeSeriesRequest timeSeries)
	{
		HistoricData[] assetPrices = await Helper(assets, timeSeries);
		double[] amounts = new double[assets.Length];
		for (int i = 0; i < assets.Length; i++) amounts[i] = assets[i].Amount;
		HistoricData total = Historic.Total(assetPrices, amounts);

		return Ok(new HistoricDataResponce(total));
	}

	[HttpPost("prices")]
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
			HistoricData prices = await GetFromMOEXAPI.Prices(assets[i].Market, assets[i].Secid, timeSeries.Frequency, timeSeries.Period);
			result[i] = prices;
		}
		return result;
	}
}