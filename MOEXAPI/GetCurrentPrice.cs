using System.Text.Json.Nodes;

namespace FinCalc.MOEXAPI
{
	public static partial class GetFromMOEXAPI
    {
        public static async Task<double> CurrentPrice(string id)
        {
            string url = $"https://iss.moex.com/iss/engines/stock/markets/shares/boards/TQBR/securities/{id}.json?iss.only=marketdata";
			string response = await Client.GetStringAsync(url);
			JsonNode? json = JsonNode.Parse(response)["marketdata"]["data"][0][12] ?? throw new UnexpectedMoexResponce(response);
            return json.GetValue<double>();
        }
    }
}