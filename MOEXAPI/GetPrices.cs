using FinCalc.DataStructures;

namespace FinCalc.MOEXAPI
{
	public static partial class Get
    {
        public static async Task<HistoricalData> Prices(string market, string id, int years)
        {
            DateTime start = DateTime.Today.AddYears(-years);
			
			string url = $"https://iss.moex.com/iss/engines/stock/markets/{market}/securities/{id}/candles.json?from={start:yyyy-MM-dd}&interval=7&iss.reverse=true";
			string response = await Client.GetStringAsync(url);
			JsonNode? json = JsonNode.Parse(response)["candles"]["data"] ?? throw new UnexpectedMoexResponce(response);
            
            int length = json.AsArray().Count;
			HistoricalData result = new(id, length, 7);
			for (int i = 0; i < length; i++)
            {
                //[7] - end date, [1] - close price
                result.Dates[i] = json[i][7].GetValue<string>();
                result.Values[i] = json[i][1].GetValue<double>();
            }
			return result;
        }
    }
}