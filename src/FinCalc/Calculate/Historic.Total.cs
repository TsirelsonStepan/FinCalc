using FinCalc.DataStructures;

namespace FinCalc.Calculate;

public static partial class Historic
{
    //
    //Find the total historical values of portfolio
    //FIX: this method needs some improvments regarding summing all series
    public static HistoricData Total(HistoricData[] assetsPrices, double[] amounts)
    {
        int commonFrequency = assetsPrices[0].Frequency;
        int earliestDate = 0;
        int longestSeries = 0;
        for (int i = 0; i < assetsPrices.Length; i++)
        {
            if (assetsPrices[i].Frequency != commonFrequency) throw new Exception("Different values of frequency during GetTotalHistoricValues()");
            earliestDate = Math.Max(earliestDate, assetsPrices[i].Dates[^1]);
            longestSeries = Math.Max(longestSeries, assetsPrices[i].Values.Length);
        }

        double?[] values = new double?[longestSeries];
        for (int i = 0; i < longestSeries; i++)
        {
            values[i] = 0;
            for (int j = 0; j < assetsPrices.Length; j++)
            {
                if (assetsPrices[j].Values.Length <= i) continue;
                if (assetsPrices[j].Values[i] == null)
                {
                    values[i] = 0;
                    break;
                }
                values[i] += assetsPrices[j].Values[i]! * amounts[j];
            }
            if (values[i] == 0) values[i] = null;
        }
        // here fix using dates of first asset
        HistoricData result = new("Portfolio", commonFrequency, earliestDate, assetsPrices[0].Dates, values);

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