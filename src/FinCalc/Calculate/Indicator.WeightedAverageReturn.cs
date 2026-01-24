namespace FinCalc.Calculate;

public static partial class Indicator
{
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