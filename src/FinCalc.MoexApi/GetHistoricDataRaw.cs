using System.Text.Json.Nodes;
using FinCalc.MoexApi.Enums;
using FinCalc.MoexApi.Models;

namespace FinCalc.MoexApi;

public partial class MoexClient
{
	public async Task<IReadOnlyList<HistoricDataPoint>> GetHistoricDataRaw(SecurityInfo securityInfo, Frequency frequency, DateTime startDate)
	{
		Dictionary<Frequency, int> EnumToMoexFreq = new()
		{
			{ Frequency.Daily, 24 },
			{ Frequency.Weekly, 7 },
			{ Frequency.Monthly, 31 },
			{ Frequency.Quarterly, 4 }
		};

		string url = "https://iss.moex.com/iss/";
		if (securityInfo.Engine != null) url += $"engines/{securityInfo.Engine}";
		if (securityInfo.Market != null) url += $"/markets/{securityInfo.Market}";
		if (securityInfo.Board != null) url += $"/boards/{securityInfo.Board}";
		url += $"/securities/{securityInfo.Secid}";
		url += $"/candles.json?from={startDate:yyyy-MM-dd}&interval={EnumToMoexFreq[frequency]}";

		List<HistoricDataPoint> result = [];
		DateTime currentDate = DateTime.Today;
		while (DateTime.Compare(currentDate, startDate) > 0)
		{
			string newUrl = url + $"&start={result.Count}";
			string response = await Client.GetStringAsync(newUrl);
			JsonArray addJson = JsonNode.Parse(response)?["candles"]?["data"]?.AsArray() ?? throw new InvalidDataException($"Unexpected MOEX response while trying to get historic data: {response}");
			
			foreach (JsonNode? item in addJson)
			{
				HistoricDataPoint newPoint = new()
				{
					ClosePrice = item![1]!.GetValue<double>(),
					CloseDateTime = GetDateTimeFromNode(item!),
				};
				result.Add(newPoint);
			}

			if (addJson.Count < 500) break;//moex returns batches of 500 rows of data, so less then 500 means last batch
		}
		return result;
	}

	private static DateTime GetDateTimeFromNode(JsonNode node)
	{
		string[] dateTimeStrArr = node[7]!.GetValue<string>().Split(' ');
		string[] dateStrArr = dateTimeStrArr[0].Split('-');
		//string[] timeStrArr = ;
		DateTime result = new(
			Convert.ToInt16(dateStrArr[0]),
			Convert.ToInt16(dateStrArr[1]),
			Convert.ToInt16(dateStrArr[2])
		);
		return result;
	}
}