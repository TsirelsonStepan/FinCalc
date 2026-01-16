namespace FinCalc.DataStructures
{	
	public partial class Portfolio
	{
		//pre-assignment fields
		public AssetInPortfolio[] Assets { get; set; } = [];
		public AssetInPortfolio Benchmark { get; set; } = new AssetInPortfolio();

		//assignment fields
		public HistoricData? HistoricBenchmarkPrices { get; set; }
		public HistoricData[] AssetsHistoricPrices { get; set; } = [];
		public double? RiskFreeRate { get; set; }

		//after-assignment fields
		public double? Beta { get; set; }
		public double? WeightedAverageReturn { get; set; }
		public double? ExpectedReturn { get; set; }
		public HistoricData? TotalHistoricValues { get; set; }

		public Portfolio() {}

		public Portfolio(AssetInPortfolio[] _assets, AssetInPortfolio _benchmark)
		{
			VerifyAssets(_assets);
			Assets = _assets;
			Benchmark = _benchmark;
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