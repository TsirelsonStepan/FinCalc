using System.Text.Json;

namespace FinCalc.AlphaVantageAPI
{
	class AlphaVantageAPI
	{
		private static readonly string domain = "https://www.alphavantage.co/query?";
		private static readonly string api_key = "Q757JLDN2ICYX2HN";

		private static async Task<string> GETReporting(string ticker, string function)
		{
			HttpClient client = new();
			string url = domain + function + $"&symbol={ticker}" + $"&apikey={api_key}";
			//string response = await client.GetStringAsync(url);
			
			
			string response = "";
			if (function == "function=CASH_FLOW") response = File.ReadAllText("./cash_flow_statement.json");
			else if (function == "function=BALANCE_SHEET") response = File.ReadAllText("./balance_sheet.json");
			else if (function == "function=INCOME_STATEMENT") response = File.ReadAllText("./income_statement_query.json");
			
			
			string last_annual_report = GetLastPeriod(response);
			return last_annual_report;
		}

		public static string GETCashFlow(string ticker)
		{
			string cash_flow = GETReporting(ticker, "function=CASH_FLOW").Result;
			return cash_flow;
		}
		public static string GETBalanceSheet(string ticker)
		{
			string balance_sheet = GETReporting(ticker, "function=BALANCE_SHEET").Result;
			return balance_sheet;
		}
		public static string GETIncomeStatement(string ticker)
		{
			string income_statement = GETReporting(ticker, "function=INCOME_STATEMENT").Result;
			return income_statement;
		}
		
		private static string GetLastPeriod(string reporting)
		{
			JsonDocument json = JsonDocument.Parse(reporting);
			JsonElement annual_periods = json.RootElement.GetProperty("annualReports");
			string last_period = annual_periods[0].ToString();
			return last_period;
		}
	}
}