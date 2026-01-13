using FinCalc.DataStructures;

namespace FinCalc.Calculate
{
	public partial class BaseIndicator
	{
		static async Task<double> Beta(AssetInPortfolio asset)
        {
            HistoricData marketReturns = Returns(await MOEXAPI.Get.Prices("index", "IMOEX", 5));
            HistoricData returns = Returns(await MOEXAPI.Get.Prices(asset.Market, asset.Secid, 5));
            if (marketReturns.Length != returns.Length)
            {
                returns = returns.FillMissing(marketReturns.Dates);
            }

            double meanReturn = returns.Values.Average();
            double meanMarketReturn = marketReturns.Values.Average();

            double sumXY = 0;
            double sumXX = 0;

            for (int i = 0; i < marketReturns.Length; i++)
            {
                double dx = returns.Values[i] - meanReturn;
                double dy = marketReturns.Values[i] - meanMarketReturn;

                sumXY += dx * dy;
                sumXX += dx * dx;
            }
            double slope = sumXY / sumXX;
            //double intercept = meanMarketReturn - slope * meanReturn;

            return slope;
        }

        public static async Task<double> PortfolioBeta(AssetInPortfolio[] assets)
        {
			double sumOfBetas = 0;
			double totalWeight = 0;
			for (int i = 0; i < assets.Length; i++)
			{
				double beta = await Beta(assets[i]);
				
				sumOfBetas += beta * assets[i].Amount;
				totalWeight += assets[i].Amount;
			}
			return sumOfBetas / totalWeight;
        }
	}
}