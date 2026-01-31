using FinCalc.DataStructures;

namespace FinCalc.Calculate;

public static partial class Historic
{
	//
	//Find the total historical values of portfolio
	//FIX: find a way to work with different dates series
	public static HistoricData Total(CustomContext context, IReadOnlyList<HistoricData> assetsRawPrices, double[] amounts)
	{
		Frequency commonFrequency = assetsRawPrices[0].Frequency;
		DateTime earliestDate = DateTime.Today;
		int longestSeries = 0;
		HistoricData[] assetsPrices = new HistoricData[assetsRawPrices.Count];
		for (int i = 0; i < assetsRawPrices.Count; i++)
		{
			if ((int)assetsRawPrices[i].Frequency != (int)commonFrequency) throw new Exception("Different values of frequency during GetTotalHistoricValues()");
			assetsPrices[i] = FitDates(assetsRawPrices[i]);
			if (DateTime.Compare(earliestDate, assetsPrices[i].Dates[^1]) > 0) earliestDate = assetsPrices[i].Dates[^1];
			longestSeries = Math.Max(longestSeries, assetsPrices[i].Values.Count);
		}

		double?[] values = new double?[longestSeries];
		for (int i = 0; i < longestSeries; i++)
		{
			values[i] = 0;
			for (int j = 0; j < assetsPrices.Length; j++)
			{
				if (assetsPrices[j].Values.Count <= i) continue;
				if (assetsPrices[j].Values[i] == null)
				{
					values[i] = 0;
					break;
				}
				if (assetsPrices[0].Dates[i] != assetsPrices[j].Dates[i])
				{
					context.AddWarning($"Dates of portfolio assets do not match for {assetsPrices[j].Name}.");
				}
				values[i] += assetsPrices[j].Values[i]! * amounts[j];
			}
			if (values[i] == 0) values[i] = null;
		}

		HistoricData result = new("Portfolio", commonFrequency, assetsPrices[0].Dates, values);

		return result;
	}
}

/*
		for (int i = 0; i < assetPrices.Length; i++)
		{
			foreach (KeyValuePair<int, double?> item in assetPrices[i].Data)
			{
				if (!result.ContainsKey(item.Key)) result[item.Key] = 0;
				result[item.Key] += item.Value * amounts[i];
			}
		}
*/