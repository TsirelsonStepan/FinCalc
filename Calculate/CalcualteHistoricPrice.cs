using FinCalc.DataStructures;

namespace FinCalc.Calculate
{
	public partial class BaseIndicator
	{
		public static HistoricData HistoricAveragePrice(Asset[] portfolio, HistoricData[] assets_data, HistoricData benchmark)
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

			Dictionary<string, double> assetsAmounts;
			double totalAmount;
			(assetsAmounts, totalAmount) = GetAssetsAmounts(portfolio);
			
			HistoricData averageData = new("Portfolio", commonLength, commonInterval);
			for (int i = 0; i < commonLength; i++)
			{
				double averagePrice = 0;
				for (int j = 0; j < assets_data.Length; j++)
				{
					averagePrice += assets_data[j].Values[i] * assetsAmounts[assets_data[j].Name];
				}
				averagePrice /= totalAmount;
				averageData.Values[i] = averagePrice;
				averageData.Dates[i] = assets_data[0].Dates[i];
			}
			return averageData;
		}

		static (Dictionary<string, double>, double) GetAssetsAmounts(Asset[] portfolio)
		{
			Dictionary<string, double> result = [];
			double totalAmount = 0;
			for (int i = 0; i < portfolio.Length; i++)
			{
				result[portfolio[i].Secid] = portfolio[i].Amount;
				totalAmount += portfolio[i].Amount;
			}
			return (result, totalAmount);
		}
	}
}