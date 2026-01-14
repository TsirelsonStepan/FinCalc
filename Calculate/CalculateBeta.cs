using FinCalc.DataStructures;

namespace FinCalc.Calculator
{
	public partial class Calculate
	{
		public static async Task<double> Beta(HistoricData assetReturns, HistoricData marketReturns)
        {
            if (marketReturns.Values.Length != assetReturns.Values.Length)
            {
                throw new Exception("The length or asset vaues does not match the length of market values");
            }

            double meanReturn = assetReturns.Values.Average();
            double meanMarketReturn = marketReturns.Values.Average();

            double sumXY = 0;
            double sumXX = 0;

            for (int i = 0; i < marketReturns.Values.Length; i++)
            {
                double dx = assetReturns.Values[i] - meanReturn;
                double dy = marketReturns.Values[i] - meanMarketReturn;

                sumXY += dx * dy;
                sumXX += dx * dx;
            }
            double slope = sumXY / sumXX;
            //double intercept = meanMarketReturn - slope * meanReturn;

            return slope;
        }
	}
}