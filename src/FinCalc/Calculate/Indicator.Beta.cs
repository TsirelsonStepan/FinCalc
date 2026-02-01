using FinCalc.Models;

namespace FinCalc.Calculate;

public static partial class Indicator
{
	public static double Beta(HistoricData assetReturns, HistoricData benchmarkReturns)
	{
		//prevent unnecessary calculation for identical data
		if (assetReturns.Name == benchmarkReturns.Name) return 1;
		
		int length = Math.Min(assetReturns.Values.Count, benchmarkReturns.Values.Count);
		double[] alignedAssetReturns = new double[length];
		double[] alignedBenchmarkReturns = new double[length];
		int newLength = 0;
		for (int i = 0; i < length; i++)
		{
			if (assetReturns.Values[i] == null || benchmarkReturns.Values[i] == null) continue;
			if (assetReturns.Dates[i] == benchmarkReturns.Dates[i])
			{
				//can use ! because if above check null
				alignedAssetReturns[i] = assetReturns.Values[i]!.Value;
				alignedBenchmarkReturns[i] = benchmarkReturns.Values[i]!.Value;
				newLength++;
			}
		}
		double beta = Basic.LinearRegressionSlope(alignedAssetReturns[0..newLength], alignedBenchmarkReturns[0..newLength]);
		return beta;
	}
}