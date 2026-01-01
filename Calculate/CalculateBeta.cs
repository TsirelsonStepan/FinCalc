namespace FinCalc.Calculate
{
	public partial class BaseIndicator
	{
		public static async Task<(double, string)> Beta(string id)
        {
            string note = "";

            Dictionary<string, double> marketReturns = await MOEXAPI.Get.Prices("index", "IMOEX", 5);
            Dictionary<string, double> returns = await MOEXAPI.Get.Prices("shares", id, 5);

            double meanReturn = returns.Values.Average();
            double meanMarketReturn = marketReturns.Values.Average();

            double sumXY = 0;
            double sumXX = 0;

            double latestReturn = 0;
            for (int j = 0; j < marketReturns.Count; j++)
            {
                if (!returns.ContainsKey(marketReturns.Keys.ToArray()[j]))
                {
                    returns[marketReturns.Keys.ToArray()[j]] = latestReturn;
                    note += $"The set of prices for {id} was incomplete, some values were extrapolated";
                }

                double dx = returns.Values.ToArray()[j] - meanReturn;
                double dy = marketReturns.Values.ToArray()[j] - meanMarketReturn;

                sumXY += dx * dy;
                sumXX += dx * dx;

                latestReturn = returns.Values.ToArray()[j];
            }
            double slope = sumXY / sumXX;
            //double intercept = meanMarketReturn - slope * meanReturn;

            return (slope, note);
        }
	}
}