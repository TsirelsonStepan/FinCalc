using FinCalc.DataStructures;
using System.Text.Json.Nodes;

namespace FinCalc.MOEXAPI
{
	public static partial class GetFromMOEXAPI
	{
		public static async Task<Asset[]> SecuritiesList(string query, string market)
		{
			string url = $"https://iss.moex.com/iss/securities.json?q={query}&is_trading=1&engine=stock&market={market}";
			string response;

			try
			{
				response = await Client.GetStringAsync(url);
			}
			catch (HttpRequestException)
			{
				throw new Exception("No connectioin");
			}
			catch (TaskCanceledException)
			{
				throw new Exception("Timeout");
			}

			JsonNode? json = JsonNode.Parse(response);
			JsonArray companiesData = json?["securities"]?["data"]?.AsArray() ?? throw new Exception("companiesData in Get.SecuritiesList == null");
			Asset[] companies = new Asset[companiesData.Count];

			for (int i = 0; i < companiesData.Count; i++)
			{
				JsonArray? companyData = companiesData[i]?.AsArray();
				if (companyData == null) continue;
                Asset company = new(
                    companyData[0]?.GetValue<string>() ?? "No Secid", //secid
                    companyData[1]?.GetValue<string>() ?? "No Shortname", //shortname
                    companyData[3]?.GetValue<string>() ?? "No Name", //name
					companyData[7]?.GetValue<string>() ?? "No Description" //description
                    //Engine = companyData[11].GetValue<string>().Split('_')[0],
                    //Market = companyData[11].GetValue<string>().Split('_')[1]
				);
                companies[i] = company;
			}

			return companies;
		}
	}
}