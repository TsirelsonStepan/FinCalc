using Microsoft.AspNetCore.Mvc;

using FinCalc.Models;
using FinCalc.Models.DTOs;
using FinCalc.Calculate;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[Route("indicators")]
public class IndicatorsController : ControllerBase
{
	[HttpPost("war")]
	[ProducesResponseType(typeof(( double, IReadOnlyList<string> )), StatusCodes.Status200OK)]
	public async Task<ActionResult<( double, IReadOnlyList<string> )>> GetWAR([FromBody] IReadOnlyList<AssetInPortfolio> portfolio, [FromQuery] TimeSeriesRequest request)
	{
		CustomContext context = new();
		double[] annualReturns = new double[portfolio.Count];
		double[] weights = new double[portfolio.Count];
		for (int i = 0; i < portfolio.Count; i++)
		{
			HistoricData prices = await IRemoteAPI.FromString(portfolio[i].Source.Api).Prices(
				context,
				portfolio[i].Source.AssetPath!,
				request.Frequency!.Value,
				request.Period!.Value);
			HistoricData returns = Historic.Returns(prices);
			double annualReturn = Indicator.AnnualReturn(returns);
			annualReturns[i] = annualReturn;
			weights[i] = portfolio[i].Amount;
		}

		return Ok(new { data = Basic.WeightedAverage(annualReturns, weights), notes = context.GetNotes() });
	}

	[HttpPost("capm")]
	[ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
	public async Task<ActionResult<double>> GetCAPM([FromBody] CAPMRequest request)
	{
		CustomContext context = new();
		Frequency frequency = request.Benchmark!.TimeSeries!.Frequency!.Value;
		int period = request.Benchmark.TimeSeries.Period!.Value;
		HistoricData benchmarkPrices = await IRemoteAPI.FromString(request.Benchmark.Source.Api).Prices(//GetPricesRaw in the future
			context,
			request.Benchmark.Source.AssetPath,
			frequency,
			period);
		HistoricData benchmarkReturns = Historic.Returns(benchmarkPrices);

		List<double> betas = [];
		List<double> weights = [];
		for (int i = 0; i < request.Assets!.Count; i++)
		{
			HistoricData prices = await IRemoteAPI.FromString(request.Assets[i].Source.Api).Prices(//GetPricesRaw in the future
				context,
				request.Assets[i].Source.AssetPath!,
				frequency,
				period);
			if (prices.Dates.Count == 0) continue;
			prices = Historic.FitDates(prices, benchmarkPrices.Dates[0], benchmarkPrices.Dates[^1]);
			HistoricData returns = Historic.Returns(prices);
			double beta = Indicator.Beta(returns, benchmarkReturns);
			betas.Add(beta);
			weights.Add(request.Assets[i].Amount);
		}
		double portfolioBeta = Basic.WeightedAverage(betas, weights);
		double riskFreeRate = await IRemoteAPI.FromString(request.Benchmark.Source.Api).RiskFreeRate();

		double capm = riskFreeRate + portfolioBeta * (Indicator.AnnualReturn(benchmarkReturns) - riskFreeRate);

		return Ok(new { data = capm, notes = context.GetNotes() });
	}
}
