using FinCalc.Calculator;

namespace FinCalc.DataStructures
{	
	public partial class Portfolio
	{
        public double GetBeta()
        {
			if (HistoricBenchmarkPrices == null) throw new Exception("Portfolio was not initialized properly. HistoricBenchmarkPrices is unassigned");
			if (AssetsHistoricPrices.Length == 0) throw new Exception("Portfolio was not initialized properly. AssetsHistoricPrices is unassigned");
			double sumOfBetas = 0;
			double totalWeight = 0;
			HistoricData marketReturns = Calculate.Returns(HistoricBenchmarkPrices);
			double meanMarketReturn = Calculate.AnnualizeReturns(marketReturns);
			for (int i = 0; i < Assets.Length; i++)
			{
            	HistoricData assetReturns = Calculate.Returns(AssetsHistoricPrices[i]);
				double beta = Calculate.Beta(assetReturns, marketReturns);
				sumOfBetas += beta * Assets[i].Amount;
				totalWeight += Assets[i].Amount;
			}
			return sumOfBetas / totalWeight;
        }

		public double GetWeightedAverageReturn()
		{
			Dictionary<string, double> assetsSecidWeigthPairs = [];
			for (int i = 0; i < Assets.Length; i++)
			{
				assetsSecidWeigthPairs[Assets[i].Secid] = Assets[i].Amount;
			}

			Dictionary<double, double> assetsReturnWeightPairs = [];
			for (int i = 0; i < AssetsHistoricPrices.Length; i++)
			{
				double annualReturn = Calculate.AnnualizeReturns(Calculate.Returns(AssetsHistoricPrices[i]));
				assetsReturnWeightPairs[annualReturn] = assetsSecidWeigthPairs[AssetsHistoricPrices[i].Name];
			}
			double wAPR = Calculate.WeightedAverageReturn(assetsReturnWeightPairs);
			return wAPR;
		}

		public double GetCAPM(double RiskFreeRate, double Beta, double marketAnnualReturn)
		{
			return 1 + RiskFreeRate + (marketAnnualReturn - 1 - RiskFreeRate) * Beta;
		}
    }
}