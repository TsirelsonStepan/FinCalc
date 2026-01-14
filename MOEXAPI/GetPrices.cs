using FinCalc.DataStructures;

namespace FinCalc.MOEXAPI
{
	public static partial class GetFromMOEXAPI
    {
        public static async Task<HistoricData> Prices(string market, string id, int freq = 7, int periods = 52)
        {
            Dictionary<int, int> daysToInterval = new()
            {
                {7, 7},
                {90, 4},
            };
            DateTime start = DateTime.Today.AddDays(-freq * periods);
			
			string url = $"https://iss.moex.com/iss/engines/stock/markets/{market}/securities/{id}/candles.json?from={start:yyyy-MM-dd}&interval={daysToInterval[freq]}&iss.reverse=true";
			string response = await Client.GetStringAsync(url);
			JsonNode? json = JsonNode.Parse(response)["candles"]["data"] ?? throw new UnexpectedMoexResponce(response);
            
            int count = json.AsArray().Count;
			HistoricData result = new(id, count);
            DateTime today = DateTime.Today;
			for (int i = 0; i < count; i++)
            {
                //[7] - end date, [1] - close price
                string[] date_string = json[i][7].GetValue<string>().Split(' ')[0].Split('-');
                DateTime date = new(Convert.ToInt16(date_string[0]), Convert.ToInt16(date_string[1]), Convert.ToInt16(date_string[2]));
                result.Dates[i] = Convert.ToInt16((today - date).TotalDays);
                result.Values[i] = json[i][1].GetValue<double>();
            }
            result.Dates[0] = 0; //Fix negative values because moex returns future periods as last price of asset
			return result;
        }
    }
}