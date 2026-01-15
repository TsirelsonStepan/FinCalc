using FinCalc.Calculator;
using FinCalc.MOEXAPI;

namespace FinCalc.DataStructures
{	
	public partial class Portfolio
	{
        public async Task<double> GetBeta()
        {
			double sumOfBetas = 0;
			double totalWeight = 0;
			HistoricData marketReturns = Calculate.Returns(await GetFromMOEXAPI.Prices("index", "IMOEX", 7, 52 * 5));
			double meanMarketReturn = await GetFromMOEXAPI.AverageAnnualReturn("index", "IMOEX", 7, 52 * 5);
			for (int i = 0; i < Assets.Length; i++)
			{
            	HistoricData assetReturns = Calculate.Returns(await GetFromMOEXAPI.Prices(Assets[i].Market, Assets[i].Secid, 7, 52 * 5));
				double beta = await Calculate.Beta(
					assetReturns,
					await GetFromMOEXAPI.AverageAnnualReturn(Assets[i].Market, Assets[i].Secid, 7, 52 * 5),
					marketReturns, meanMarketReturn);
				sumOfBetas += beta * Assets[i].Amount;
				totalWeight += Assets[i].Amount;
			}
			return sumOfBetas / totalWeight;
        }

		public async Task<double> GetWeightedAverageReturn()
		{
			Dictionary<string, double> assetsSecidWeigthPairs = [];
			for (int i = 0; i < Assets.Length; i++)
			{
				assetsSecidWeigthPairs[Assets[i].Secid] = Assets[i].Amount;
			}
			Dictionary<double, double> assetsReturnWeightPairs = [];
			for (int i = 0; i < Assets.Length; i++)
			{
				double annualReturn = await GetFromMOEXAPI.AverageAnnualReturn(Assets[i].Market, Assets[i].Secid);
				assetsReturnWeightPairs[annualReturn] = assetsSecidWeigthPairs[Assets[i].Secid];
			}
			double wAPR = Calculate.WeightedAverageReturn(assetsReturnWeightPairs);
			return wAPR;
		}

		public async Task<double> GetCAPM()
		{
			double rfrate = await GetFromMOEXAPI.RFRate();
			double beta = Beta ?? throw new Exception("beta is null");
			double marketAnnualReturn = await GetFromMOEXAPI.AverageAnnualReturn("index", "IMOEX", 90, 40);
			return 1 + rfrate + (marketAnnualReturn - 1 - rfrate) * beta;
		}
    }
}