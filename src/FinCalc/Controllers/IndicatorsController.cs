using FinCalc.DataStructures;
using FinCalc.Calculate;
using FinCalc.RemoteAPIs;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("api/indicators")]
public class IndicatorsController : ControllerBase
{
	private readonly IRemoteAPI API = new MOEXAPI();

	[HttpPost("war")]
	[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
	public async Task<ActionResult<double>> GetWAR([FromBody] AssetInPortfolio[] portfolio, [FromQuery] TimeSeriesRequest request)
	{
		double[] annualReturns = new double[portfolio.Length];
		double[] weights = new double[portfolio.Length];
		for (int i = 0; i < portfolio.Length; i++)
		{
			HistoricData prices = await API.Prices(
				portfolio[i].Market!,
				portfolio[i].Secid!,
				request.Frequency!.Value,
				request.Period!.Value);
			HistoricData returns = Historic.Returns(prices);
			double annualReturn = Indicator.AnnualReturn(returns);
			annualReturns[i] = annualReturn;
			weights[i] = portfolio[i].Amount;
		}

		return Ok(Basic.WeightedAverage(annualReturns, weights));
	}

	[HttpPost("capm")]
	[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
	public async Task<ActionResult<double>> GetCAPM([FromBody] CAPMRequest request)
	{
		int frequency = request.Benchmark!.TimeSeries!.Frequency!.Value;
		int period = request.Benchmark!.TimeSeries!.Period!.Value;
		HistoricData benchmarkPrices = await API.Prices(
			request.Benchmark.Source!.Market!,
			request.Benchmark.Secid!,
			frequency,
			period);
		HistoricData benchmarkReturns = Historic.Returns(benchmarkPrices);

		double[] betas = new double[request.Assets!.Length];
		double[] weights = new double[request.Assets.Length];
		for (int i = 0; i < request.Assets.Length; i++)
		{
			HistoricData prices = await API.Prices(
				request.Assets[i].Market!,
				request.Assets[i].Secid!,
				frequency,
				period);
			HistoricData returns = Historic.Returns(prices);
			double beta = Indicator.Beta(returns, benchmarkReturns);
			betas[i] = beta;
			weights[i] = request.Assets[i].Amount;
		}
		double portfolioBeta = Basic.WeightedAverage(betas, weights);
		double riskFreeRate = await API.RiskFreeRate();

		double capm = riskFreeRate + portfolioBeta * (Indicator.AnnualReturn(benchmarkReturns) - riskFreeRate);

		return Ok(capm);
	}
}
