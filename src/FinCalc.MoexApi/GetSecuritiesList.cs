using System.Text.Json.Nodes;

using FinCalc.MoexApi.Models;

namespace FinCalc.MoexApi;

public partial class MoexClient
{
	public async Task<IReadOnlyList<SecurityInfo>> GetSecuritiesList(string query)
	{
		string[] queryArr = query.Split('/');
		string url = $"https://iss.moex.com/iss/securities.json?";

		if (queryArr.Length == 0 || queryArr.Length > 4) throw new InvalidDataException($"Unexpected MOEX query format: {query}");
		if (queryArr.Length >= 1) url += $"&q={queryArr[^1]}";
		if (queryArr.Length >= 2) url += $"&engine={queryArr[^2]}";
		if (queryArr.Length >= 3) url += $"&market={queryArr[^3]}";
		if (queryArr.Length >= 4) url += $"&board={queryArr[^4]}";

		string response;

		try { response = await Client.GetStringAsync(url); }

		catch (HttpRequestException) { throw new Exception("No connectioin"); }
		catch (TaskCanceledException) { throw new Exception("Timeout"); }

		JsonNode? json = JsonNode.Parse(response);
		JsonArray companiesData = json?["securities"]?["data"]?.AsArray() ?? throw new InvalidDataException($"Unexpected MOEX response while trying to get securoties list: {response}");
		SecurityInfo[] companies = new SecurityInfo[companiesData.Count];

		for (int i = 0; i < companiesData.Count; i++)
		{
			JsonArray? companyData = companiesData[i]?.AsArray();
			if (companyData == null) continue;
			SecurityInfo company = new()
			{
				Engine = companyData[11]?.GetValue<string>().Split('_')[0] ?? "No engine",
				Market = companyData[11]?.GetValue<string>().Split('_')[1] ?? "No Market",
				Board = companyData[12]?.GetValue<string>() ?? "No Board",
				Secid = companyData[0]?.GetValue<string>() ?? "No Secid",
				Shortname = companyData[1]?.GetValue<string>() ?? "No Shortname",
				Name = companyData[3]?.GetValue<string>() ?? "No Name",
				Description = companyData[7]?.GetValue<string>() ?? "No Description"
			};
			companies[i] = company;
		}

		return companies;
	}
}