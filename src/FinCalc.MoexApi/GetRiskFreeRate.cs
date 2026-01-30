using System.Text.Json.Nodes;

namespace FinCalc.MoexApi;

public partial class MoexClient
{
	public async Task<double> GetRiskFreeRate()
	{
		string url = $"https://iss.moex.com/iss/engines/stock/zcyc.json";
		string response = await Client.GetStringAsync(url);
		//[3] selects 1-year maturity, [3] selects value ([2] - maturity in years, [0] and [1] date and time)
		double result = JsonNode.Parse(response)?["yearyields"]?["data"]?[3]?[3]?.GetValue<double?>() ?? throw new Exception("Error in RiskFreeRate() result == null");
		return result / 100;
	}
}