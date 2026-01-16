using FinCalc.DataStructures;

namespace FinCalc.Calculator
{
	public static partial class Calculate
    {
        public static double AnnualizeReturns(HistoricData historicReturns)
        {
			double average = 1;
			for (int i = 0; i < historicReturns.Dates.Length; i++)
			{
				average *= 1 + historicReturns.Values[i] ?? 0;
			}
			average = 1 + (Math.Pow(average, (double)(1d / historicReturns.Values.Length)) - 1) * (365d / historicReturns.Frequency);
			return average;
		}
    }
}