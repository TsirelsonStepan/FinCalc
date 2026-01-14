using FinCalc.DataStructures;

namespace FinCalc.Calculator
{
	public partial class Calculate
	{		
        public static HistoricData Returns(HistoricData prices)
		{
			HistoricData returns = new(prices.Secid, prices.Values.Length);
			
			int newLength = prices.Dates.Length - 1;
			for (int i = 0; i < newLength; i++)
			{
				returns.Dates[i] = prices.Dates[i];
				returns.Values[i] = prices.Values[i] / prices.Values[i + 1];
			}
			returns.Dates[newLength] = prices.Dates[newLength];
			returns.Values[newLength] = 1;
			return returns;
		}

		public static double AnnualReturn(HistoricData prices)
		{
			HistoricData returns = Returns(prices);

			double average = 1;
			for (int i = 0; i < returns.Dates.Length - 1; i++)
			{
				int interval = -(returns.Dates[i] - returns.Dates[i + 1]);
				average *= 1 + (returns.Values[i] - 1) / interval * 7;
			}
			average = 1 + (Math.Pow(average, 1d / returns.Values.Length) - 1) * 52;
			return average;
		}

		public static double WeightedAverageReturn(Dictionary<HistoricData, double> assetsDataWeights)
		{
			double sumOfReturns = 0;
			double totalWeight = 0;

			foreach (KeyValuePair<HistoricData, double> item in assetsDataWeights)
			{
				sumOfReturns += AnnualReturn(item.Key) * item.Value;
				totalWeight += item.Value;
			}

			return sumOfReturns / totalWeight;
		}

		public static double CAPM(HistoricData assetPrices, double beta, double rfrate)
		{
			double rm = AnnualReturn(assetPrices) - 1;	
			return 1 + rfrate + (rm - rfrate) * beta;
		}
    }
}