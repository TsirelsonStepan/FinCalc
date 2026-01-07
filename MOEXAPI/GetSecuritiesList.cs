using FinCalc.DataStructures;

namespace FinCalc.MOEXAPI
{
	public static partial class Get
	{
		public static async Task<Asset[]> SecuritiesList(string query)
		{
			string url = $"https://iss.moex.com/iss/securities.json?q={query}&is_trading=1&engine=stock&market=shares";
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
			JsonArray companiesData = json["securities"]["data"].AsArray();
			Asset[] companies = new Asset[companiesData.Count];

			for (int i = 0; i < companiesData.Count; i++)
			{
				JsonArray companyData = companiesData[i].AsArray();
                Asset company = new()
                {
                    Secid = companyData[0].GetValue<string>(),
                    Shortname = companyData[1].GetValue<string>(),
                    Name = companyData[3].GetValue<string>(),
					Description = companyData[7].GetValue<string>(),
                    //Engine = companyData[11].GetValue<string>().Split('_')[0],
                    //Market = companyData[11].GetValue<string>().Split('_')[1]
                };
                companies[i] = company;
			}

			return companies;
		}
	}
}