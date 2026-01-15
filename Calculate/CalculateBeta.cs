using FinCalc.DataStructures;

namespace FinCalc.Calculator
{
	public partial class Calculate
	{
		public static async Task<double> Beta(HistoricData assetReturns, double meanAssetReturns, HistoricData marketReturns, double meanMarketReturns)
        {
            if (marketReturns.Values.Length != assetReturns.Values.Length)
            {
                throw new Exception("The length or asset vaues does not match the length of market values");
            }

            double sumXY = 0;
            double sumXX = 0;

            for (int i = 0; i < marketReturns.Values.Length; i++)
            {
                if (assetReturns.Values[i] == null || marketReturns.Values[i] == null) continue;
                double dx = assetReturns.Values[i] ?? 1 - meanAssetReturns;
                double dy = marketReturns.Values[i] ?? 1 - meanMarketReturns;

                sumXY += dx * dy;
                sumXX += dx * dx;
            }
            double slope = sumXY / sumXX;
            //double intercept = meanMarketReturn - slope * meanReturn;

            return slope;
        }
	}
}