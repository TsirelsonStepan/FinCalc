using FinCalc.DataStructures;

namespace FinCalc.Calculate;

public static partial class Indicator
{
	public static double AnnualReturns(HistoricData historicReturns)
	{
		double average = 1;
		for (int i = 0; i < historicReturns.Dates.Length; i++)
		{
			average *= 1 + (historicReturns.Values[i] ?? 0);
		}
		average = (Math.Pow(average, 1d / historicReturns.Values.Length) - 1d) * (365d / historicReturns.Frequency);
		return average;
	}
}