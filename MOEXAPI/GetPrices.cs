namespace FinCalc.MOEXAPI
{
	public static partial class Get
    {
        public static async Task<Dictionary<string, double>> Prices(string market, string id, int years)
        {
            DateTime start = DateTime.Today.AddYears(-years);
			
			string url = $"https://iss.moex.com/iss/engines/stock/markets/{market}/securities/{id}/candles.json?from={start:yyyy-MM-dd}&interval=7&iss.reverse=true";
			string response = await Client.GetStringAsync(url);
			JsonNode? json = JsonNode.Parse(response)["candles"]["data"] ?? throw new UnexpectedMoexResponce(response);
            
			Dictionary<string, double> result = [];
			for (int i = 0; i < json.AsArray().Count; i++)
            {
                //[7] - end date, [1] - close price
                result[json[i][7].GetValue<string>()] = json[i][1].GetValue<double>();
            }
			return result;
        }
    }
}