using FinCalc.DataStructures;

namespace FinCalc.Calculator
{
	public partial class Calculate
	{
		public static double Beta(HistoricData assetReturns, HistoricData marketReturns)
        {
            if (marketReturns.Values.Length != assetReturns.Values.Length)
            {
                throw new HistoricDataLengthIsDifferent("During calculations of Beta, length of asset HistoricData was not equal to length of market HistoricData. Behaviour for this case is in development.");
            }

            double meanAssetReturns = assetReturns.Values.Average() ?? throw new Exception("null values in returns HistoricalData prevented Beta() function from calculating meanRetuen");
            double meanMarketReturns = marketReturns.Values.Average() ?? throw new Exception("null values in returns HistoricalData prevented Beta() function from calculating meanRetuen");

            double sumXY = 0;
            double sumXX = 0;

            for (int i = 0; i < marketReturns.Values.Length; i++)
            {
                if (assetReturns.Values[i] == null || marketReturns.Values[i] == null) continue;
                double dx = assetReturns.Values[i] ?? 0 - meanAssetReturns;
                double dy = marketReturns.Values[i] ?? 0 - meanMarketReturns;

                sumXY += dx * dy;
                sumXX += dx * dx;
            }
            double slope = sumXY / sumXX;
            //double intercept = meanMarketReturn - slope * meanReturn;

            return slope;
        }
	}
}