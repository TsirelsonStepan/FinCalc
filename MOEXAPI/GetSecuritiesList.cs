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
			JsonArray companies_data = json["securities"]["data"].AsArray();
			Asset[] companies = new Asset[companies_data.Count];

			for (int i = 0; i < companies_data.Count; i++)
			{
				JsonArray company_data = companies_data[i].AsArray();
                Asset company = new()
                {
                    Secid = company_data[0].GetValue<string>(),
                    Shortname = company_data[1].GetValue<string>(),
                    Name = company_data[3].GetValue<string>(),
                    //Engine = company_data[11].GetValue<string>().Split('_')[0],
                    //Market = company_data[11].GetValue<string>().Split('_')[1]
                };
                companies[i] = company;
			}

			return companies;
		}
	}
}