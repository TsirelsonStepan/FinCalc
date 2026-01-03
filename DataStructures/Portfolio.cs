namespace FinCalc.DataStructures
{	
	public partial class Portfolio(Asset[] _assets)
	{
		Asset[] Assets { get; set; } = _assets;

		public double? WeightedAveragePortfolioReturn { get; set; }
		public double? ExpectedPortfolioReturn { get; set; }
		public double? PortfolioVariance { get; set; }
		public double? PortfolioBeta { get; set; }
		public string Notes { get; set; } = "";

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

				Notes += note + '\n';
			}
			PortfolioBeta = sumOfBetas / totalWeight;
		}

		public async Task CalcualteWeightedAverageReturn()
		{
			double sumOfReturns = 0;
			double totalWeight = 0;
			for (int i = 0; i < Assets.Length; i++)
			{
				double returns = Calculate.BaseIndicator.AnnualReturn(
					Calculate.BaseIndicator.Returns(
						await MOEXAPI.Get.Prices("shares", Assets[i].Secid, 1)));
				
				sumOfReturns += returns * Assets[i].Amount;
				totalWeight += Assets[i].Amount;
			}
			WeightedAveragePortfolioReturn = sumOfReturns / totalWeight;
		}

		public async Task CalculateExpectedReturn()
		{
			double Rm = Calculate.BaseIndicator.AnnualReturn(
				Calculate.BaseIndicator.Returns(
					await MOEXAPI.Get.Prices("index", "IMOEX", 10)));
			Rm -= 1;		
			
			double Rf = MOEXAPI.Get.RiskFreeRate;
			ExpectedPortfolioReturn = 1 + Rf + (Rm - Rf) * PortfolioBeta;
		}
	}
}