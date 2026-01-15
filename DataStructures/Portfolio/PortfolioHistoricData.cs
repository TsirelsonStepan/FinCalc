using FinCalc.MOEXAPI;

namespace FinCalc.DataStructures
{	
	public partial class Portfolio
	{
		public async Task<HistoricData[]> GetAssetsHistoricPrices(int freq = 7, int period = 52)
		{
			
			HistoricData[] historicData = new HistoricData[Assets.Length];
			for (int i = 0; i < Assets.Length; i++)
			{
				historicData[i] = await GetFromMOEXAPI.Prices(Assets[i].Market, Assets[i].Secid, freq, period);
			}
			return historicData;
		}

		public async Task<HistoricData> GetTotalHistoricValues(int freq = 7, int period = 52)
		{
			HistoricData[] historicPrices;
			if (AssetsHistoricPrices.Length == 0) historicPrices = await GetAssetsHistoricPrices(freq, period);
			else historicPrices = AssetsHistoricPrices;

			Dictionary<string, double> AssetAmountPairs = [];
			for (int i = 0; i < Assets.Length; i++)
			{
				AssetAmountPairs[Assets[i].Secid] = Assets[i].Amount;
			}

			int longestSeries = 0;
			for (int i = 0; i < historicPrices.Length; i++)
			{
				longestSeries = Math.Max(longestSeries, historicPrices[i].Values.Length);
			}

			HistoricData totalValues = new("Portfolio", longestSeries);
			for (int i = 0; i < longestSeries; i++)
			{
				double totalValue = 0;
				for (int j = 0; j < Assets.Length; j++)
				{
					if (i >= historicPrices[j].Values.Length) continue;
					totalValue += historicPrices[j].Values[i] ?? 0 * AssetAmountPairs[historicPrices[j].Secid];
				}
				totalValues.Values[i] = totalValue;
				totalValues.Dates[i] = BenchmarkHistoricData.Dates[i];
			}
			return totalValues;
		}
    }
}