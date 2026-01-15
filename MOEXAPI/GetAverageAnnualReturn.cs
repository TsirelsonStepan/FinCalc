using FinCalc.DataStructures;

namespace FinCalc.MOEXAPI
{
	public static partial class GetFromMOEXAPI
    {
        public static async Task<double> AverageAnnualReturn(string market, string id, int freq = 7, int period = 52)
        {
			HistoricData historicPrices = await Prices(market, id, freq, period);
			double average = 1;
			for (int i = 0; i < historicPrices.Dates.Length - 1; i++)
			{
				if (historicPrices.Values[i] == null || historicPrices.Values[i + 1] == null) continue;
				average *= (historicPrices.Values[i] ?? 1) / (historicPrices.Values[i + 1] ?? 1);
			}
			average = 1 + (Math.Pow(average, (double)(1d / period)) - 1) * (365d / freq);
			return average;
		}
    }
}