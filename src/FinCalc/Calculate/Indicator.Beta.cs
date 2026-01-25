using FinCalc.DataStructures;

namespace FinCalc.Calculate;

public static partial class Indicator
{
	public static double Beta(HistoricData assetReturns, HistoricData benchmarkReturns)
	{
		int length = Math.Min(assetReturns.Values.Length, benchmarkReturns.Values.Length);
		double[] alignedAssetReturns = new double[length];
		double[] alignedBenchmarkReturns = new double[length];
		for (int i = 0; i < length; i++)
		{
			if (assetReturns.Values[i] == null || benchmarkReturns.Values[i] == null) continue;
			if (assetReturns.Dates[i] == benchmarkReturns.Dates[i])
			{
				//can use ! because if above check null
				alignedAssetReturns[i] = assetReturns.Values[i]!.Value;
				alignedBenchmarkReturns[i] = benchmarkReturns.Values[i]!.Value;
			}
		}
		double beta = Basic.LinearRegressionSlope(alignedAssetReturns, alignedBenchmarkReturns);
		return beta;
	}
}