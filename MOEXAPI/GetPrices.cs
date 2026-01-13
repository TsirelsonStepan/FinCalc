using FinCalc.DataStructures;

namespace FinCalc.MOEXAPI
{
	public static partial class Get
    {
        public static async Task<HistoricData> Prices(string market, string id, int interval = 7, int periods = 52)
        {
            Dictionary<int, int> daysToInterval = new()
            {
                {7, 7},
                {90, 4},
            };
            DateTime start = DateTime.Today.AddDays(-interval * periods);
			
			string url = $"https://iss.moex.com/iss/engines/stock/markets/{market}/securities/{id}/candles.json?from={start:yyyy-MM-dd}&interval={daysToInterval[interval]}&iss.reverse=true";
			string response = await Client.GetStringAsync(url);
			JsonNode? json = JsonNode.Parse(response)["candles"]["data"] ?? throw new UnexpectedMoexResponce(response);
            
            int length = json.AsArray().Count;
			HistoricData result = new(id, length, 7);
			for (int i = 0; i < length; i++)
            {
                //[7] - end date, [1] - close price
                result.Dates[i] = json[i][7].GetValue<string>().Split(' ')[0];
                result.Values[i] = json[i][1].GetValue<double>();
            }
			return result;
        }
    }
}