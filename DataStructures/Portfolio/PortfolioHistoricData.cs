namespace FinCalc.DataStructures
{	
	public partial class Portfolio
	{
		public HistoricData GetTotalHistoricValues()
		{
			//if (HistoricBenchmarkPrices == null) throw new Exception("Portfolio was not initialized properly. HistoricBenchmarkPrices == null");
			Dictionary<string, double> AssetAmountPairs = [];
			for (int i = 0; i < Assets.Length; i++)
			{
				AssetAmountPairs[Assets[i].Secid] = Assets[i].Amount;
			}

			int longestSeries = 0;
			int commonFreq = AssetsHistoricPrices[0].Frequency;
			int longestPeriod = 0;
			for (int i = 0; i < AssetsHistoricPrices.Length; i++)
			{
				if (AssetsHistoricPrices[i].Frequency != commonFreq) throw new Exception("Different values of frequency during GetTotalHistoricValues()");
				longestSeries = Math.Max(longestSeries, AssetsHistoricPrices[i].Values.Length);
				longestPeriod = Math.Max(longestPeriod, AssetsHistoricPrices[i].Period);
			}

			HistoricData totalValues = new("Portfolio", longestSeries, commonFreq, longestPeriod);
			for (int i = 0; i < longestSeries; i++)
			{
				double totalValue = 0;
				for (int j = 0; j < Assets.Length; j++)
				{
					if (i >= AssetsHistoricPrices[j].Values.Length) continue;
					totalValue += (AssetsHistoricPrices[j].Values[i] ?? 0) * AssetAmountPairs[AssetsHistoricPrices[j].Name];
				}
				totalValues.Values[i] = totalValue;
				totalValues.Dates[i] = AssetsHistoricPrices[0].Dates[i];//HistoricBenchmarkPrices.Dates[i];
			}
			totalValues.RealDates = totalValues.GetRealDates();
			return totalValues;
		}
    }
}