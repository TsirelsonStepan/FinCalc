using FinCalc.DataStructures;

namespace FinCalc.Calculate
{
	public partial class BaseIndicator
	{
		public static HistoricData HistoricAveragePrice(HistoricData[] assets_data)
		{
			int commonLength = assets_data[0].Length;
			int commonInterval = assets_data[0].Interval;
			for (int i = 1; i < assets_data.Length; i++)
			{
				if (assets_data[i].Length != commonLength) throw new Exception("Assets comprising portfolio have different dataset length");
				if (assets_data[i].Interval != commonInterval) throw new Exception("Assets comprising portfolio have different dataset interval");
			}
			
			double[] averageData = new double [commonLength];
			for (int i = 0; i < commonLength; i++)
			{
				double averagePrice = 0;
				for (int j = 0; j < assets_data.Length; j++)
				{
					averagePrice += assets_data[j].Values[i];
				}
				averagePrice /= assets_data.Length;
				averageData[i] = averagePrice;
			}
			HistoricData averagePortfolio = new("Portfolio", commonLength, commonInterval, assets_data[0].Dates, averageData);
			return averagePortfolio;
		}
	}
}