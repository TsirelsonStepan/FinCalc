using FinCalc.MoexApi;
using FinCalc.MoexApi.Models;

using FinCalc.Models;

namespace FinCalc.RemoteAPIs;

public class MoexAPI : IRemoteAPI
{
	private static readonly MoexClient Client = new();
	
	public async Task<HistoricData> Prices(CustomContext context, string assetPath, Frequency frequency, int period)
	{
		Dictionary<Frequency, MoexApi.Enums.Frequency> FrequencyMap = new()
		{
			{ Frequency.Daily, MoexApi.Enums.Frequency.Daily },
			{ Frequency.Weekly, MoexApi.Enums.Frequency.Weekly },
			{ Frequency.Monthly, MoexApi.Enums.Frequency.Monthly }
		};

		string? engine = null;
		string? market = null;
		string? board = null;
		string secid = ""; //this will never pass
		
		string[] assetPathArr = assetPath.Split('/');
		if (assetPathArr.Length == 0 || assetPathArr.Length > 4) throw new InvalidDataException($"Unexpected asset path: {assetPath}");
		if (assetPathArr.Length >= 1) secid = assetPathArr[^1];
		if (assetPathArr.Length >= 2) board = assetPathArr[^2];
		if (assetPathArr.Length >= 3) market = assetPathArr[^3];
		if (assetPathArr.Length == 4) engine = assetPathArr[^4];
		
		SecurityInfo securityInfo = new()
		{
			Engine = engine,
			Market = market,
			Board = board,
			Secid = secid
		};

		IReadOnlyList<HistoricDataPoint> rawData = await Client.GetHistoricDataRaw(
			securityInfo,
			FrequencyMap[frequency],
			DateTime.Today.AddDays(-period)
		);
		double?[] values = new double?[rawData.Count()];
		DateTime[] dates = new DateTime[rawData.Count()];
		int i = 0;
		foreach (HistoricDataPoint point in rawData)
		{
			values[i] = point.ClosePrice;
			dates[i] = point.CloseDateTime;
			i++;
		}
		HistoricData result = new(assetPath, frequency, dates, values);
		return result;
	}

	public async Task<double> CurrentPrice(string assetPath)
	{
		string[] assetPathArr = assetPath.Split('/');
		double result = await Client.GetCurrentPrice(
			new SecurityInfo()
			{
				Engine = assetPathArr[0],
				Market = assetPathArr[1],
				Board = assetPathArr[2],
				Secid = assetPathArr[3]
			}
		);
		return result;
	}

	public async Task<double> RiskFreeRate()
	{
		double result = await Client.GetRiskFreeRate();
		return result;
	}

	public async Task<IReadOnlyList<Asset>> SecuritiesList(string query)
	{
		IReadOnlyList<SecurityInfo> result = await Client.GetSecuritiesList(query);
		Asset[] assets = new Asset[result.Count];

		for (int i = 0; i < result.Count; i++)
		{
			assets[i] = new Asset()
			{
				Source = new Source()
				{
					Api = "moex",
					AssetPath = result[i].Engine + '/' + result[i].Market + '/' + result[i].Board + '/' + result[i].Secid
				},
				Shortname = result[i].Shortname,
				Name = result[i].Name,
				Description = result[i].Description
			};
		}

		return assets;
	}
}