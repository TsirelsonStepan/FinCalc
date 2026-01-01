namespace FinCalc.DataStructures
{	
	public partial class Portfolio(Asset[] _assets)
	{
		Asset[] Assets { get; set; } = _assets;

		public double? WeightedAveragePortfolioReturn { get; set; }
		public string? NotesToWeightedAveragePortfolioReturn { get; set; }

		public double? ExpectedPortfolioReturn { get; set; }
		public string? NotesToExpectedPortfolioReturn { get; set; }

		public double? PortfolioVariance { get; set; }
		public string? NotesToPortfolioVariance { get; set; }
		
		public double? PortfolioBeta { get; set; }
		public string? NotesToPortfolioBeta { get; set; }

		public void Verify()
		{
			//total amount > 0
			double totalAmount = 0;
			foreach (Asset asset in Assets) totalAmount += asset.Amount;
			if (totalAmount <= 0) throw new PortfolioSizeIsZero();
		}

		public async Task CalculateBeta()
		{
			double sumOfBetas = 0;
			double totalWeight = 0;
			for (int i = 0; i < Assets.Length; i++)
			{
				(double beta, string note) = await Calculate.BaseIndicator.Beta(Assets[i].Secid);
				
				sumOfBetas += beta * Assets[i].Amount;
				totalWeight += Assets[i].Amount;

				NotesToPortfolioBeta += note + '\n';
			}
			PortfolioBeta = sumOfBetas / totalWeight;
		}

		public async Task CalcualteWeightedAverageReturn()
		{
			double sumOfReturns = 0;
			double totalWeight = 0;
			for (int i = 0; i < Assets.Length; i++)
			{
				double returns = Calculate.BaseIndicator.AnnualReturn(await MOEXAPI.Get.Prices("shares", Assets[i].Secid, 5));
				
				sumOfReturns += returns * Assets[i].Amount;
				totalWeight += Assets[i].Amount;
			}
			PortfolioBeta = sumOfReturns / totalWeight;
		}

		public async Task CalculateExpectedReturn()
		{
			double Rm = Calculate.BaseIndicator.AnnualReturn(await MOEXAPI.Get.Prices("index", "IMOEX", 5));
			double Rf = MOEXAPI.Get.RiskFreeRate;
			ExpectedPortfolioReturn = Rf + (Rm - Rf) * PortfolioBeta;
		}
	}
}