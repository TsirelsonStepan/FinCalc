using System.Text.Json.Nodes;
using FinCalc.MoexApi.Models;

namespace FinCalc.MoexApi;

public partial class MoexClient
{
	public async Task<double> GetCurrentPrice(SecurityInfo securityInfo)
	{
		string url = $"https://iss.moex.com/iss/engines/{securityInfo.Engine}/markets/{securityInfo.Market}/boards/{securityInfo.Board}/securities/{securityInfo.Secid}.json?iss.only=marketdata";
		string response = await Client.GetStringAsync(url);
		double value = JsonNode.Parse(response)?["marketdata"]?["data"]?[0]?[12]?.GetValue<double?>()?? throw new InvalidDataException($"Unexpectex MOEX responce: {response}");
		return value;
	}
}