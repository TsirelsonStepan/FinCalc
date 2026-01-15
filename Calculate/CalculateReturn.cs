using FinCalc.DataStructures;

namespace FinCalc.Calculator
{
	public partial class Calculate
	{		
        public static HistoricData Returns(HistoricData prices)
		{
			int newLength = prices.Dates.Length - 1;
			HistoricData returns = new(prices.Secid, newLength);
			
			for (int i = 0; i < newLength; i++)
			{
				returns.Dates[i] = prices.Dates[i];
				returns.Values[i] = prices.Values[i] / prices.Values[i + 1];
			}
			return returns;
		}

		public static double WeightedAverageReturn(Dictionary<double, double> assetsAnnualReturnToWeights)
		{
			double sumOfReturns = 0;
			double totalWeight = 0;

			foreach (KeyValuePair<double, double> item in assetsAnnualReturnToWeights)
			{
				sumOfReturns += item.Key * item.Value;
				totalWeight += item.Value;
			}

			return sumOfReturns / totalWeight;
		}
    }
}