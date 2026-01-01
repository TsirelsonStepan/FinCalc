namespace FinCalc.Calculate
{
	public partial class BaseIndicator
	{		
        public static Dictionary<string, double> Returns(Dictionary<string, double> prices)
		{
			Dictionary<string, double> returns = [];
			string[] dates = prices.Keys.ToArray();
			
			for (int i = 0; i < prices.Count - 1; i++)
			{
				returns[dates[i]] = prices[dates[i]] / prices[dates[i + 1]];
			}
			return returns;
		}

		static Dictionary<string, double> NormilizeReturns(Dictionary<string, double> returns)
		{
			//add missing period
			return returns;
		}

		public static double AnnualReturn(Dictionary<string, double> prices)
		{//implement
			string date = prices.Keys.ToArray()[0];
			return prices[date];
		}
    }
}

/*
			int daysInYear = DateTime.IsLeapYear(DateTime.Today.Year) ? 366 : 365;
			int daysElapsed = (DateTime.Today - new DateTime(DateTime.Today.Year, 1, 1)).Days + 1;

			int firstQuarterIndex = GetFirstQuarterIndex(json);

			double currentYearPriceChange = json[0][1].GetValue<double>() / json[firstQuarterIndex][0].GetValue<double>();
			
			//annualize current year price change
			double totalGrowth = Math.Pow(1 + currentYearPriceChange, daysInYear / daysElapsed) - 1;
			
			//CAGR
			for (int i = 0; i < past_years; i++)
			{
				double previousYearPriceChange = json[firstQuarterIndex + 1][1].GetValue<double>() / json[firstQuarterIndex + 4][0].GetValue<double>();
				totalGrowth *= 1 + previousYearPriceChange;
			}
			double cagr = Math.Pow(totalGrowth, 1 / (past_years + 1)) - 1;
			
			return cagr;

			//TO DO
			//add handle in case not enough periods
*/