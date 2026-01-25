using System.Text.Json.Nodes;

namespace FinCalc.RemoteAPIs;

public partial class MOEXAPI
{
	public async Task<double?> CurrentPrice(string secid)
	{
		string url = $"https://iss.moex.com/iss/engines/stock/markets/shares/boards/TQBR/securities/{secid}.json?iss.only=marketdata";
		string response = await Client.GetStringAsync(url);
		JsonNode json = JsonNode.Parse(response)?["marketdata"]?["data"]?[0]?[12] ?? throw new UnexpectedMoexResponce(response);
		return json.GetValue<double?>();
	}
}