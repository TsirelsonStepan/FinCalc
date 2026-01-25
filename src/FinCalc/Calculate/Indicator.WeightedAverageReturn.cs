namespace FinCalc.Calculate;

public static partial class Basic
{
	public static double WeightedAverage(double[] values, double[] weights)
	{
		double sumOfValues = 0;
		double totalWeight = 0;

		for (int i = 0; i < values.Length; i++)
		{
			sumOfValues += values[i] * weights[i];
			totalWeight += weights[i];
		}

		return sumOfValues / totalWeight;
	}
}