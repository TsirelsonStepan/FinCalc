using FinCalc.DataStructures;

namespace FinCalc.Calculator
{
	public partial class Calculate
	{
		//Returns only non-null values, as null values of prices are transformed into return = 1, assumed no changes
        public static HistoricData Returns(HistoricData prices)
		{
			int newLength = prices.Dates.Length - 1;
			HistoricData returns = new(prices.Name, newLength, prices.Frequency, prices.Period - prices.Frequency);
			
			for (int i = 0; i < newLength; i++)
			{
				returns.Dates[i] = prices.Dates[i];
				if (prices.Values[i] == null || prices.Values[i + 1] == null)
				{
					returns.Values[i] = 0;
				}
				else returns.Values[i] = prices.Values[i] / prices.Values[i + 1] - 1;
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