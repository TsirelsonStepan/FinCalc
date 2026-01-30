using FinCalc.DataStructures;
using FinCalc.RemoteAPIs;

public interface IRemoteAPI
{
	Task<Asset[]> SecuritiesList(
		string query
	);

	Task<HistoricData> Prices(
		CustomContext context,
		string assetPath,
		Frequency frequency,
		int period
	);

	Task<double> CurrentPrice(
		string assetPath
	);

	Task<double> RiskFreeRate();

	public static IRemoteAPI FromString(string api)
	{
		if (api == "moex") return new MoexAPI();
		else if (api == "yfinance") return new YFinanceAPI();
		throw new Exception("Provided api string is not found");
	}
}