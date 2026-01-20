namespace FinCalc.DataStructures
{	
	public partial class Portfolio
	{
		//pre-assignment fields
		public AssetInPortfolio[] Assets { get; set; } = [];

		//assignment fields
		public HistoricData[] AssetsHistoricPrices { get; set; } = [];
		public double? RiskFreeRate { get; set; }

		//after-assignment fields
		public double? Beta { get; set; }
		public double? WeightedAverageReturn { get; set; }
		public double? ExpectedReturn { get; set; }
		public HistoricData? TotalHistoricValues { get; set; }

		public Portfolio() {}

		public Portfolio(AssetInPortfolio[] assets)
		{
			VerifyAssets(assets);
			Assets = assets;
		}

		private void VerifyAssets(AssetInPortfolio[] assets)
		{
			foreach (AssetInPortfolio asset in assets)
			{
				if (asset.Amount <= 0) throw new PortfolioSizeIsZero("One of the assets in portfolio was not positive");
			}
		}
	}
}