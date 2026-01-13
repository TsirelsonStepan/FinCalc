namespace FinCalc.DataStructures
{	
	partial class Portfolio
	{
		private AssetInPortfolio[] Assets { get; }

		public double? WeightedAveragePortfolioReturn { get; }
		public double? ExpectedPortfolioReturn { get; }
		public double? PortfolioVariance { get; }
		public double? PortfolioBeta { get; }
		public double RiskFreeRate { get; }
		public HistoricData BenchmarkHistoricData { get; }
		public HistoricData[] PortfolioHistoricData { get; } = [];
		public HistoricData PortfolioAverageHistoricData { get; }


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
				if (asset.Amount <= 0) throw new PortfolioSizeIsZero();
			}
		}
	}
}