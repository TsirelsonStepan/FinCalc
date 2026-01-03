namespace FinCalc.MOEXAPI
{
	public static partial class Get
	{
		private static readonly HttpClient Client = new();

		public static double RiskFreeRate { get; set; }
	}
}

/*
https://iss.moex.com/iss/index
https://iss.moex.com/iss/reference/

Search for securities by partial name - https://iss.moex.com/iss/securities.json?q=

General information about security issuance - https://iss.moex.com/iss/securities/Ozon.json

Precise information about trade period (daily, hourly, etc.) - 
https://iss.moex.com/iss/history/engines/stock/markets/shares/securities/Ozon.json?from=2025-09-01&till=2025-11-20
https://iss.moex.com/iss/reference/439

https://iss.moex.com/iss/engines/stock/markets/shares/securities/{ticker}/trades.json - The most recent 5000 individual trades
https://iss.moex.com/iss/reference/425

https://iss.moex.com/iss/engines/stock/markets/shares/securities/{ticker}/candles.json - all the prices (open, close, etc.)
https://iss.moex.com/iss/reference/341

https://iss.moex.com/iss/engines/stock/markets/shares/securities/{ticker}/ - https://iss.moex.com/iss/reference/347

https://iss.moex.com/iss/history/engines/stock/zcyc
*/