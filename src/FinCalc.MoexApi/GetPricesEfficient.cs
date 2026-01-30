/*using System.Text.Json.Nodes;

namespace FinCalc.RemoteAPIs;

public partial class MOEXAPI
{
	//Efficient
	public async Task<HistoricData> Prices(CustomContext context, string asset, Frequency frequency, int period)
	{
		Dictionary<Frequency, int> EnumFreqToMoexFreq = new()
		{
			{Frequency.Daily, 24},
			{Frequency.Weekly, 7},
			{Frequency.Monthly, 31}
		};
		string[] assetPath = asset.Split('/');
		DateTime start = DateTime.Today.AddDays(-period * 1.1);
		string url = $"https://iss.moex.com/iss/engines/stock/markets/{assetPath[^2]}/securities/{assetPath[^1]}/candles.json?from={start:yyyy-MM-dd}&interval={EnumFreqToMoexFreq[frequency]}&iss.reverse=true";

		List<JsonArray> finalJson = [];
		int totalCount = 0;
		while (true)
		{
			string newUrl = url + $"&start={totalCount}";
			string response = await Client.GetStringAsync(newUrl);
			JsonArray addJson = JsonNode.Parse(response)?["candles"]?["data"]?.AsArray() ?? throw new UnexpectedMoexResponce(response);
			finalJson!.Add(addJson);
			totalCount += addJson.Count;
			if (addJson.Count < 500) break;//moex returns batches of 500 rows of data, so less then 500 means last batch
		}

		string[] latestDateStr = finalJson!.First()[0]![7]!.GetValue<string>().Split(' ')[0].Split('-');
		DateTime latestDate = new(
			Convert.ToInt16(latestDateStr[0]),
			Convert.ToInt16(latestDateStr[1]),
			Convert.ToInt16(latestDateStr[2]));
		
		double latestPrice = finalJson!.First()[0]![1]!.GetValue<double>();

		List<DateTime> dates = [];
		List<double?> values = [latestPrice];

		if (DateTime.Compare(latestDate, DateTime.Today) < 0) dates.Add(latestDate);
		else dates.Add(DateTime.Today);

		finalJson!.First().RemoveAt(0);//removing latest date as it is already added

		foreach (JsonArray array in finalJson)
			for (int i = 0; i < array.Count; i++)
			{
				DateTime expectedDate = StepByFrequency(latestDate, frequency);
				if ((DateTime.Today - expectedDate).TotalDays > period) break;
				string[] currentDateStr = array[i]![7]!.GetValue<string>().Split(' ')[0].Split('-');
				DateTime currentDate = new(
					Convert.ToInt16(currentDateStr[0]),
					Convert.ToInt16(currentDateStr[1]),
					Convert.ToInt16(currentDateStr[2]));
				double currentPrice = array[i]![1]!.GetValue<double>();

				double? newPrice;
				if (DateTime.Compare(currentDate, expectedDate) == 0) newPrice = currentPrice;
				else if (DateTime.Compare(currentDate, expectedDate) > 0)
				{
					context.AddWarning($"Moex date: {currentDate:yyyy-MM-dd} was out of frequency, skipped");
					continue;//real date was earlier the expected, meaning moex is incosistent with frequency, therefore this value is skipped and cycle move on to next value
				}
				else if (DateTime.Compare(currentDate, expectedDate) < 0)
				{
					if (DateTime.Compare(currentDate, StepByFrequency(expectedDate, frequency)) > 0)//real date is between last and expected
					{
						newPrice = AverageValue(latestDate, latestPrice, currentDate, currentPrice, expectedDate);
						context.AddNote($"Value for {expectedDate:yyyy-MM-dd} was filled with average between {latestDate:yyyy-MM-dd} and {currentDate:yyyy-MM-dd}");
					}
					else//real date is too far in the past - skip
					{
						//newPrice = AverageValue(latestDate, latestPrice, currentDate, currentPrice, expectedDate);
						newPrice = null;
						context.AddNote($"Value was skipped due to Moex date being out of frequency: {currentDate:yyyy-MM-dd}");
					}
				}
				else throw new Exception("unexpected");

				dates.Add(expectedDate);
				values.Add(newPrice);
				latestDate = expectedDate;
				if (newPrice == null) i--;//stay on the same data point if real price value wasn't used
			}
		return new HistoricData(asset, frequency, period, dates.ToArray(), values.ToArray());
	}

	private static DateTime StepByFrequency(DateTime date, Frequency frequency, int dir = -1)
	{
		if (frequency == Frequency.Daily) return date.AddDays(1 * dir);
		else if (frequency == Frequency.Weekly) return date.AddDays(7 * dir);
		else if (frequency == Frequency.Monthly)
		{
			DateTime d = date.AddMonths(1 * dir);
			return new DateTime(d.Year, d.Month, DateTime.DaysInMonth(d.Year, d.Month));//MOEX return monthly data on last day of month
		}
		else throw new ArgumentException($"Unsupported frequency: {frequency}");
	}

	//
	//recentDate/Value are already added, earlierDate/Value are in the past and target date must be in between
	private static double? AverageValue(DateTime recentDate, double? recentValue, DateTime earlierDate, double? earlierValue, DateTime targetDate)
	{
		double shareOfRecent = DateTime.Compare(recentDate, targetDate);
		double shareOfEarlier = DateTime.Compare(targetDate, earlierDate);
		return (recentValue * shareOfEarlier + earlierValue * shareOfRecent) / (shareOfRecent + shareOfEarlier);
	}
}*/