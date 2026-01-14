using FinCalc.MOEXAPI;

namespace FinCalc.DataStructures
{	
	public partial class Portfolio
	{
		public async Task<HistoricData[]> GetAssetsHistoricPrices(int freq = 7, int periods = 52)
		{
			HistoricData[] historicData = new HistoricData[Assets.Length];
			for (int i = 0; i < Assets.Length; i++)
			{
				historicData[i] = await GetFromMOEXAPI.Prices(Assets[i].Market, Assets[i].Secid, freq, periods);
			}
			return historicData;
		}

		public async Task<HistoricData> GetTotalHistoricValues(int freq = 7, int periods = 52)
		{
			HistoricData[] historicPrices;
			if (AssetsHistoricPrices.Length == 0) historicPrices = await GetAssetsHistoricPrices(freq, periods);
			else historicPrices = AssetsHistoricPrices;

			Dictionary<string, double> AssetAmountPairs = [];
			for (int i = 0; i < Assets.Length; i++)
			{
				AssetAmountPairs[Assets[i].Secid] = Assets[i].Amount;
			}

			HistoricData totalValues = new("Portfolio", periods);
			for (int i = 0; i < periods; i++)
			{
				double totalValue = 0;
				for (int j = 0; j < Assets.Length; j++)
				{
					totalValue += historicPrices[j].Values[i] * AssetAmountPairs[historicPrices[j].Secid];
				}
				totalValues.Values[i] = totalValue;
				totalValues.Dates[i] = BenchmarkHistoricData.Dates[i];
			}
			return totalValues;
		}
    }
}