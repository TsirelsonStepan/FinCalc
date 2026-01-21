using FinCalc.DataStructures;
using System.Text.Json.Nodes;

namespace FinCalc.MOEXAPI
{
	public static partial class GetFromMOEXAPI
	{
		public static async Task<HistoricData> Prices(string market, string id, int freq = 7, int length = 52)
		{
			Dictionary<int, int> daysToInterval = new()
			{
				{1, 24},
				{7, 7},
				{31, 31},
				{90, 4},
			};
			DateTime start = DateTime.Today.AddDays(-freq * length);
			
			string url = $"https://iss.moex.com/iss/engines/stock/markets/{market}/securities/{id}/candles.json?from={start:yyyy-MM-dd}&interval={daysToInterval[freq]}&iss.reverse=true";
			JsonArray finalJson = [];
			DateTime today = DateTime.Today;
			string[] date;
			DateTime lastDate;
			int totalLengthDays = 0;
			
			while (totalLengthDays < length)
			{
				string newUrl = url + $"&start={finalJson?.Count}";
				string response = await Client.GetStringAsync(newUrl);
				JsonArray addJson = JsonNode.Parse(response)?["candles"]?["data"]?.AsArray() ?? throw new UnexpectedMoexResponce(response);
				foreach (JsonNode? item in addJson) finalJson?.Add(item?.DeepClone());
				JsonNode? dateNode = finalJson?.Last()?[7];
				if (dateNode != null)
				{
					date = dateNode.GetValue<string>().Split(' ')[0].Split('-');
					lastDate = new(Convert.ToInt16(date[0]), Convert.ToInt16(date[1]), Convert.ToInt16(date[2]));
				}
				else
				{
					lastDate = start;
				}

				totalLengthDays = Convert.ToInt16((today - lastDate).TotalDays);
				if (addJson.Count < 500) break; //500 seems to be limit on one time data retrieval 
			}

			int addedValues = 0;
			int daysCounter = 0;
			int[] dates = new int[length];
			double?[] values = new double?[length];
			
			for (int i = 0; i < length; i++)
			{
				if (i - addedValues >= finalJson?.Count)
				{
					dates[i] = daysCounter;
					daysCounter += freq;
					continue;
				}
				JsonNode? currentDateNode = finalJson?[i - addedValues]?[7];
				int currentDaysPassed;
				if (currentDateNode != null)
				{
					string[] currentDateStr = currentDateNode.GetValue<string>().Split(' ')[0].Split('-');
					DateTime currentDate = new(Convert.ToInt16(currentDateStr[0]), Convert.ToInt16(currentDateStr[1]), Convert.ToInt16(currentDateStr[2]));
					currentDaysPassed = (int)(today - currentDate).TotalDays;
				}
				else
				{
					currentDaysPassed = daysCounter;
				}

				if (currentDaysPassed - daysCounter < freq)
				{
					daysCounter = currentDaysPassed;
					dates[i] = daysCounter;
					values[i] = finalJson?[i - addedValues]?[1]?.GetValue<double>();
				}
				else
				{
					dates[i] = daysCounter;
					addedValues++;
				}
				daysCounter += freq;
			}
			dates[0] = 0;
			HistoricData result;
			
			result = new(id, length, freq, freq * length, dates, values);
			
			return result;
		}
	}
}