using System.Text.Json;

namespace FinCalc.DataStructures
{	
	public class Portfolio
	{
		public Portfolio() {}

		public AssetInPortfolio[] Assets { get; set; } = [];

		public double? WeightedAveragePortfolioReturn { get; set; }
		public double? ExpectedPortfolioReturn { get; set; }
		public double? PortfolioVariance { get; set; }
		public double? PortfolioBeta { get; set; }
		public double? RiskFreeRate { get; set; }
		public HistoricData? BenchmarkHistoricData { get; set; }
		public HistoricData[] PortfolioHistoricData { get; set; } = [];
		public HistoricData? PortfolioAverageHistoricData { get; set; }


		private Portfolio(AssetInPortfolio[] _assets, HistoricData Benchmark, double RFRate, HistoricData[] historicData, double beta, double wAPR, double capm)
		{
			VerifyAssets(_assets);
			Assets = _assets;
			BenchmarkHistoricData = Benchmark;
			RiskFreeRate = RFRate;
			PortfolioHistoricData = historicData;
			WeightedAveragePortfolioReturn = wAPR;
			ExpectedPortfolioReturn = capm;

			PortfolioAverageHistoricData = Calculate.BaseIndicator.HistoricPortfolioValue(_assets, PortfolioHistoricData, Benchmark);
			PortfolioBeta = beta;
		}

		public static async Task<Portfolio> CreateAsync(AssetInPortfolio[] _assets)
		{
			HistoricData[] historicData = new HistoricData[_assets.Length];
			for (int i = 0; i < _assets.Length; i++)
			{
				historicData[i] = await MOEXAPI.Get.Prices(_assets[i].Market, _assets[i].Secid, 1);
			}
			HistoricData benchmark = await MOEXAPI.Get.Prices("index", "IMOEX", 1);
			double rfrate = await MOEXAPI.Get.RFRate();

			double beta = await Calculate.BaseIndicator.PortfolioBeta(_assets);
			double wAPR = await Calculate.BaseIndicator.WeightedAverageReturn(_assets);
			double capm = await Calculate.BaseIndicator.CAPM(beta, rfrate);

			Portfolio portfolio = new(_assets, benchmark, rfrate, historicData, beta, wAPR, capm);
			return portfolio;
		}

		public void VerifyAssets(AssetInPortfolio[] assets)
		{
			foreach (AssetInPortfolio asset in assets)
			{
				if (asset.Amount <= 0) throw new PortfolioSizeIsZero("One of the assets in portfolio was not positive");
			}
		}

        static readonly JsonSerializerOptions options = new()
        {
			PropertyNameCaseInsensitive = true
		};

		public static Portfolio Deserialize(string portfolio)
		{
			return JsonSerializer.Deserialize<Portfolio>(portfolio, options) ?? throw new PortfolioSizeIsZero("portfolio json deserializaion failed");
		}
		public static string Serialize(Portfolio portfolio)
		{
			return JsonSerializer.Serialize(portfolio, options);
		}
	}
}