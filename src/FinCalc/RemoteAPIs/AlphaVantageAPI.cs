/*
using System.Text.Json;

namespace FinCalc.RemoteAPIs;

public class AlphaVantageAPI
{
	private static readonly string domain = "https://www.alphavantage.co/query?";
	private static readonly string apiKey = "yourApiKey";

	private static async Task<string> GETReporting(string ticker, string function)
	{
		HttpClient client = new();
		string url = domain + function + $"&symbol={ticker}" + $"&apikey={apiKey}";
		string response = await client.GetStringAsync(url);			
		
		string lastAnnualReport = GetLastPeriod(response);
		return lastAnnualReport;
	}

	public static string GETCashFlow(string ticker)
	{
		string cashFlow = GETReporting(ticker, "function=CASH_FLOW").Result;
		return cashFlow;
	}
	public static string GETBalanceSheet(string ticker)
	{
		string balanceSheet = GETReporting(ticker, "function=BALANCE_SHEET").Result;
		return balanceSheet;
	}
	public static string GETIncomeStatement(string ticker)
	{
		string incomeStatement = GETReporting(ticker, "function=INCOME_STATEMENT").Result;
		return incomeStatement;
	}
	
	private static string GetLastPeriod(string reporting)
	{
		JsonDocument json = JsonDocument.Parse(reporting);
		JsonElement annualPeriods = json.RootElement.GetProperty("annualReports");
		string lastPeriod = annualPeriods[0].ToString();
		return lastPeriod;
	}
}
*/