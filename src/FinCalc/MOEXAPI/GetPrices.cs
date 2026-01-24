using FinCalc.DataStructures;
using System.Text.Json.Nodes;

namespace FinCalc.MOEXAPI
{
	public static partial class GetFromMOEXAPI
	{
		public static async Task<HistoricData> Prices(string market, string id, int frequency, int period)
		{
			Dictionary<int, int> daysToInterval = new()
			{
				{1, 24},
				{7, 7},
				{30, 31},
				{90, 4},
			};
			//get period longer then desired by 1 freq (1 observation) to prevent missing early data
			DateTime start = DateTime.Today.AddDays(-(period + frequency));
			
			string url = $"https://iss.moex.com/iss/engines/stock/markets/{market}/securities/{id}/candles.json?from={start:yyyy-MM-dd}&interval={daysToInterval[frequency]}&iss.reverse=true";
			JsonArray finalJson = [];
			DateTime today = DateTime.Today;
			string[] date;
			DateTime lastDate;
			int totalLengthDays = 0;//this is created in next loop but used later
			
			//request batches of data until there no more data or desired period is covered
			while (totalLengthDays < period)
			{
				//request and parse data
				string newUrl = url + $"&start={finalJson!.Count}";
				string response = await Client.GetStringAsync(newUrl);
				JsonArray addJson = JsonNode.Parse(response)?["candles"]?["data"]?.AsArray() ?? throw new UnexpectedMoexResponce(response);
				
				//concat with previous data
				foreach (JsonNode? item in addJson) finalJson!.Add(item?.DeepClone());
				
				//trust MOEX API not to return null nodes
				JsonNode dateNode = finalJson!.Last()![7]!;

				date = dateNode.GetValue<string>().Split(' ')[0].Split('-');
				lastDate = new(Convert.ToInt16(date[0]), Convert.ToInt16(date[1]), Convert.ToInt16(date[2]));

				totalLengthDays = Convert.ToInt16((today - lastDate).TotalDays);
				//500 seems to be limit on one time data retrieval, meaning if there is less the 500 it is the last batch
				if (addJson.Count < 500) break;
			}

			int addedValues = 0;
			int daysCounter = 0;
			int desiredLength = period / frequency;
			int[] dates = new int[desiredLength];
			double?[] values = new double?[desiredLength];
			
			for (int i = 0; i < desiredLength; i++)
			{
				//continue filling in null values if there is no more data left but desired period is not reached
				if (i - addedValues >= finalJson?.Count)
				{
					dates[i] = daysCounter;
					values[i] = null;
					daysCounter += frequency;
					continue;
				}

				//real (not expected) number of days passed
				JsonNode currentDateNode = finalJson![i - addedValues]![7]!; //trusting MOEX again
				string[] currentDateStr = currentDateNode.GetValue<string>().Split(' ')[0].Split('-');
				DateTime currentDate = new(Convert.ToInt16(currentDateStr[0]), Convert.ToInt16(currentDateStr[1]), Convert.ToInt16(currentDateStr[2]));
				int currentDaysPassed = (int)(today - currentDate).TotalDays;

				//check if the real dates are not too far in the past meaning no values skipped and real dates correspond to expected (approximately)
				if (currentDaysPassed - daysCounter < frequency)
				{
					daysCounter = currentDaysPassed;
					//trust MOEX
					values[i] = finalJson![i - addedValues]![1]!.GetValue<double>();
					dates[i] = daysCounter;
				}
				else //meaning real dates are too far in the past and need to fill in missed dates with nulls
				{
					dates[i] = daysCounter;
					values[i] = null;
					addedValues++;
				}
				daysCounter += frequency;
			}
			dates[0] = 0;
			HistoricData result = new(id, frequency, period, dates, values);
			
			return result;
		}
	}
}