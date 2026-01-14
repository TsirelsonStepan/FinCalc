using FinCalc.MOEXAPI;

namespace FinCalc.DataStructures
{	
	public partial class Portfolio
	{
		public AssetInPortfolio[] Assets { get; set; } = [];

		public double? WeightedAverageReturn { get; set; }
		public double? ExpectedReturn { get; set; }
		public double? Beta { get; set; }
		public double? RiskFreeRate { get; set; }
		public HistoricData? BenchmarkHistoricData { get; set; }
		public HistoricData[] AssetsHistoricPrices { get; set; } = [];
		public HistoricData? TotalHistoricValues { get; set; }

		public Portfolio() {}

		private Portfolio(AssetInPortfolio[] _assets, HistoricData Benchmark)
		{
			VerifyAssets(_assets);
			Assets = _assets;
			BenchmarkHistoricData = Benchmark;
		}

		public static async Task<Portfolio> CreateAsync(AssetInPortfolio[] _assets)
		{
			HistoricData benchmark = await GetFromMOEXAPI.Prices("index", "IMOEX", 7, 52 * 5);

			Portfolio portfolio = new(_assets, benchmark);

			portfolio.AssetsHistoricPrices = await portfolio.GetAssetsHistoricPrices();
			portfolio.TotalHistoricValues = await portfolio.GetTotalHistoricValues();

			portfolio.Beta = await portfolio.GetBeta();
			
			portfolio.WeightedAverageReturn = await portfolio.GetWeightedAverageReturn();
			portfolio.ExpectedReturn = await portfolio.GetCAPM();
			
			return portfolio;
		}

		public void VerifyAssets(AssetInPortfolio[] assets)
		{
			foreach (AssetInPortfolio asset in assets)
			{
				if (asset.Amount <= 0) throw new PortfolioSizeIsZero("One of the assets in portfolio was not positive");
			}
		}
	}
}