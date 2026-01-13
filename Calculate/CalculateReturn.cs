using FinCalc.DataStructures;

namespace FinCalc.Calculate
{
	public partial class BaseIndicator
	{		
        public static HistoricData Returns(HistoricData prices)
		{
			HistoricData returns = new(prices.Name, prices.Length - 1, prices.Interval);
			
			for (int i = 0; i < prices.Length - 1; i++)
			{
				returns.Dates[i] = prices.Dates[i]; // OPTIMIZE
				returns.Values[i] = prices.Values[i] / prices.Values[i + 1];
			}
			return returns;
		}

		public static double AnnualReturn(HistoricData returns)
		{
			double average = 1;
			for (int i = 0; i < returns.Length; i++)
			{
				average *= returns.Values[i];
			}
			average = Math.Pow(average, 1d / returns.Length);
			return 1 + (average - 1) * (365 / returns.Interval);
		}

		public static async Task<double> WeightedAverageReturn(AssetInPortfolio[] assets)
		{
			double sumOfReturns = 0;
			double totalWeight = 0;
			for (int i = 0; i < assets.Length; i++)
			{
				double returns = AnnualReturn(Returns(await MOEXAPI.Get.Prices(assets[i].Market, assets[i].Secid, 1)));
				
				sumOfReturns += returns * assets[i].Amount;
				totalWeight += assets[i].Amount;
			}
			return sumOfReturns / totalWeight;
		}

		public static async Task<double> CAPM(double beta, double rfrate)
		{
			double rm = AnnualReturn(Returns(await MOEXAPI.Get.Prices("index", "IMOEX", 10))) - 1;	
			return 1 + rfrate + (rm - rfrate) * beta;
		}
    }
}

/*
			int daysInYear = DateTime.IsLeapYear(DateTime.Today.Year) ? 366 : 365;
			int daysElapsed = (DateTime.Today - new DateTime(DateTime.Today.Year, 1, 1)).Days + 1;

			int firstQuarterIndex = GetFirstQuarterIndex(json);

			double currentYearPriceChange = json[0][1].GetValue<double>() / json[firstQuarterIndex][0].GetValue<double>();
			
			//annualize current year price change
			double totalGrowth = Math.Pow(1 + currentYearPriceChange, daysInYear / daysElapsed) - 1;
			
			//CAGR
			for (int i = 0; i < pastYears; i++)
			{
				double previousYearPriceChange = json[firstQuarterIndex + 1][1].GetValue<double>() / json[firstQuarterIndex + 4][0].GetValue<double>();
				totalGrowth *= 1 + previousYearPriceChange;
			}
			double cagr = Math.Pow(totalGrowth, 1 / (pastYears + 1)) - 1;
			
			return cagr;

			//TO DO
			//add handle in case not enough periods
*/