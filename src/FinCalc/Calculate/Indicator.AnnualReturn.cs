using FinCalc.DataStructures;

namespace FinCalc.Calculate;

public static partial class Indicator
{
	public static double AnnualReturn(HistoricData historicReturns)
	{
		double average = 1;
		for (int i = 0; i < historicReturns.Dates.Count; i++)
		{
			average *= 1 + (historicReturns.Values[i] ?? 0);
		}
		average = (Math.Pow(average, 1d / historicReturns.Values.Count) - 1d) * (365d / (int)historicReturns.Frequency);
		return average;
	}
}