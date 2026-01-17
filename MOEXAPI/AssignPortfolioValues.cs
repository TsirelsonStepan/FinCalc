using FinCalc.DataStructures;

namespace FinCalc.MOEXAPI
{
	public static class AssignPortfolioValues
	{
		public static async Task<Portfolio> Whole(Portfolio portfolio, int freq, int length)
		{
			//string benchmarkMarket = portfolio.Benchmark.Market;
			//string benchmarkSecid = portfolio.Benchmark.Secid;
			//portfolio.HistoricBenchmarkPrices = await GetFromMOEXAPI.Prices(benchmarkMarket, benchmarkSecid, freq, length);

			int nOfAssets = portfolio.Assets.Length;
			portfolio.AssetsHistoricPrices = new HistoricData[nOfAssets];
			for (int i = 0; i < nOfAssets; i++)
			{
				string assetMarket = portfolio.Assets[i].Market;
				string assetSecid = portfolio.Assets[i].Secid;
				portfolio.AssetsHistoricPrices[i] = await GetFromMOEXAPI.Prices(assetMarket, assetSecid, freq, length);
				portfolio.AssetsHistoricPrices[i].RealDates = portfolio.AssetsHistoricPrices[i].GetRealDates();
			}

			portfolio.RiskFreeRate = await GetFromMOEXAPI.RiskFreeRate();

			return portfolio;
		}
	}
}