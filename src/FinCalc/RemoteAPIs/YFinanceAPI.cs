using OoplesFinance.YahooFinanceAPI;
using OoplesFinance.YahooFinanceAPI.Models;
using OoplesFinance.YahooFinanceAPI.Enums;

using FinCalc.DataStructures;

namespace FinCalc.RemoteAPIs;

public class YFinanceAPI : IRemoteAPI
{
	private static readonly YahooClient Client = new();

	public async Task<HistoricData> Prices(CustomContext context, string assetPath, Frequency frequency, int period)
	{
		Dictionary<Frequency, DataFrequency> FrequencyMap = new()
		{
			{ Frequency.Daily, DataFrequency.Daily },
			{ Frequency.Weekly, DataFrequency.Weekly },
			{ Frequency.Monthly, DataFrequency.Monthly }
		};
		IEnumerable<HistoricalChartInfo> rawData = await Client.GetHistoricalDataAsync(assetPath, FrequencyMap[frequency], DateTime.Today.AddDays(-period));
		double?[] values = new double?[rawData.Count()];
		DateTime[] dates = new DateTime[rawData.Count()];
		int i = rawData.Count() - 1; //reverse direction because api provide in cronological order (opposite to moex)
		foreach (HistoricalChartInfo price in rawData)
		{
			values[i] = price.Close;
			dates[i] = price.Date;
			i--;
		}
		HistoricData result = new(assetPath, frequency, dates, values);
		return result;
	}
	public async Task<double> CurrentPrice(string assetPath)
	{
		double result = (await Client.GetPriceInfoAsync(assetPath)).RegularMarketPrice.Raw ?? throw new Exception("Price not found"); //replace to Yahoo error type exception
		return result;
	}
	public async Task<double> RiskFreeRate()
	{
		double result = await CurrentPrice("^TNX"); //10 year treasury yield
		return result / 100;
	}
	public async Task<Asset[]> SecuritiesList(string query)
	{
		IEnumerable<AutoCompleteResult> assets = await Client.GetAutoCompleteInfoAsync(query);
		Asset[] results = new Asset[assets.Count()];
		int i = 0;
		foreach (AutoCompleteResult asset in assets)
		{
			AssetProfile detail = await Client.GetAssetProfileAsync(asset.Symbol);
			results[i] = new Asset()
			{
				Name = asset.Name,
				Source = new()
				{
					Api = "yfinance",
					AssetPath = asset.Symbol
				},
				Description = detail.LongBusinessSummary
			};

			i++;
		}
		return results;
	}
}