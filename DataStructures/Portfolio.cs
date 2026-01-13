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
		public HistoricData[] AssetsHistoricPrices { get; set; } = [];
		public HistoricData? TotalHistoricValues { get; set; }


		private Portfolio(AssetInPortfolio[] _assets, HistoricData Benchmark, double RFRate, double beta, double wAPR, double capm)
		{
			VerifyAssets(_assets);
			Assets = _assets;
			BenchmarkHistoricData = Benchmark;
			RiskFreeRate = RFRate;
			WeightedAveragePortfolioReturn = wAPR;
			ExpectedPortfolioReturn = capm;
			PortfolioBeta = beta;
		}

		public static async Task<Portfolio> CreateAsync(AssetInPortfolio[] _assets)
		{
			HistoricData benchmark = await MOEXAPI.Get.Prices("index", "IMOEX", 7, 52 * 5);
			double rfrate = await MOEXAPI.Get.RFRate();

			double beta = await Calculate.BaseIndicator.PortfolioBeta(_assets);
			double wAPR = await Calculate.BaseIndicator.WeightedAverageReturn(_assets);
			double capm = await Calculate.BaseIndicator.CAPM(beta, rfrate);

			Portfolio portfolio = new(_assets, benchmark, rfrate, beta, wAPR, capm);

			return portfolio;
		}

		public void VerifyAssets(AssetInPortfolio[] assets)
		{
			foreach (AssetInPortfolio asset in assets)
			{
				if (asset.Amount <= 0) throw new PortfolioSizeIsZero("One of the assets in portfolio was not positive");
			}
		}

		public async Task<HistoricData[]> GetAssetsHistoricPrices(int interval = 7, int periods = 52)
		{
			HistoricData[] historicData = new HistoricData[Assets.Length];
			for (int i = 0; i < Assets.Length; i++)
			{
				historicData[i] = await MOEXAPI.Get.Prices(Assets[i].Market, Assets[i].Secid, interval, periods);
			}
			return historicData;
		}

		public async Task<HistoricData> GetTotalHistoricValues(int interval = 7, int periods = 52)
		{
			if (AssetsHistoricPrices.Length == 0) AssetsHistoricPrices = await GetAssetsHistoricPrices(interval, periods);
			Dictionary<string, double> AssetAmountPairs = [];
			for (int i = 0; i < Assets.Length; i++)
			{
				if (AssetsHistoricPrices[i].Length != periods)
				{
					AssetsHistoricPrices[i] = AssetsHistoricPrices[i].FillMissing(BenchmarkHistoricData.Dates);
				}
				if (AssetsHistoricPrices[i].Interval != interval) throw new Exception("Assets comprising portfolio have different dataset interval");

				AssetAmountPairs[Assets[i].Secid] = Assets[i].Amount;
			}

			HistoricData totalValues = new("Portfolio", periods, interval);
			for (int i = 0; i < periods; i++)
			{
				double totalValue = 0;
				for (int j = 0; j < Assets.Length; j++)
				{
					totalValue += AssetsHistoricPrices[j].Values[i] * AssetAmountPairs[AssetsHistoricPrices[j].Secid];
				}
				totalValues.Values[i] = totalValue;
				totalValues.Dates[i] = BenchmarkHistoricData.Dates[i];
			}
			return totalValues;
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