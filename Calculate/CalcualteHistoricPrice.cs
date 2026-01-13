using FinCalc.DataStructures;

namespace FinCalc.Calculate
{
	public partial class BaseIndicator
	{
		public static HistoricData HistoricPortfolioValue(AssetInPortfolio[] portfolio, HistoricData[] assets_data, HistoricData benchmark)
		{
			int commonLength = assets_data[0].Length;
			int commonInterval = assets_data[0].Interval;
			for (int i = 1; i < assets_data.Length; i++)
			{
				if (assets_data[i].Length != commonLength)
				{
					assets_data[i] = assets_data[i].FillMissing(benchmark.Dates);
				}
				if (assets_data[i].Interval != commonInterval) throw new Exception("Assets comprising portfolio have different dataset interval");
			}

			Dictionary<string, double> assetsAmounts = [];
			for (int i = 0; i < portfolio.Length; i++)
			{
				assetsAmounts[portfolio[i].Secid] = portfolio[i].Amount;
			}
			
			HistoricData averageData = new("Portfolio", commonLength, commonInterval);
			for (int i = 0; i < commonLength; i++)
			{
				double totalValue = 0;
				for (int j = 0; j < assets_data.Length; j++)
				{
					totalValue += assets_data[j].Values[i] * assetsAmounts[assets_data[j].Name];
				}
				averageData.Values[i] = totalValue;
				averageData.Dates[i] = assets_data[0].Dates[i];
			}
			return averageData;
		}
	}
}