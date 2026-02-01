using FinCalc.Models;

namespace FinCalc.Calculate;

public static partial class Historic
{
	//
	//Returns only non-null values, as null values of prices are transformed into return = 1, assumed no changes
	public static HistoricData Returns(HistoricData prices)
	{
		DateTime[] dates = new DateTime[prices.Dates.Count - 1];
		double?[] values = new double?[prices.Dates.Count - 1];
		
		for (int i = 0; i < prices.Dates.Count - 1; i++)
		{
			dates[i] = prices.Dates[i];
			if (prices.Values[i] == null || prices.Values[i + 1] == null)
			{
				values[i] = 0;
			}
			else values[i] = prices.Values[i] / prices.Values[i + 1] - 1;
		}
		HistoricData returns = new(prices.Name, prices.Frequency, dates, values);
		return returns;
	}
}