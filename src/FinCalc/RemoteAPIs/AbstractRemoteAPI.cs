using FinCalc.DataStructures;

public interface IRemoteAPI
{
    Task<Asset[]> SecuritiesList(
        string query,
        string source
    );

    Task<HistoricData> Prices(
        string market,
        string secid,
        int frequency,
        int period
    );

    Task<double?> CurrentPrice(
        string secid
    );

    Task<double> RiskFreeRate();
}