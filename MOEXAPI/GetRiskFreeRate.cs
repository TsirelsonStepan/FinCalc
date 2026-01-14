namespace FinCalc.MOEXAPI
{
	public static partial class GetFromMOEXAPI
	{
		public static async Task<double> RFRate()
        {
			string url = $"https://iss.moex.com/iss/engines/stock/zcyc.json";
			string response = await Client.GetStringAsync(url);
			//[3] selects 1-year maturity, [3] selects value ([2] - maturity in years, [0] and [1]  date and time)
			double result = JsonNode.Parse(response)["yearyields"]["data"][3][3].GetValue<double>() / 100;
			return result;
        }
	}
}

/*

		public static async Task GetRiskFreeRate(double t)
		{
			string url = "https://iss.moex.com/iss/history/engines/stock/zcyc.json?time=23:59:59";
			string response = await HttpClient.GetStringAsync(url);
			JsonNode? json = JsonNode.Parse(response)["params"]["data"][0];
			double[] Bs = [
				json[2].GetValue<double>() / 10000d,
				json[3].GetValue<double>() / 10000d,
				json[4].GetValue<double>() / 10000d,
			];
			double T1 = json[5].GetValue<double>();
			double[] Gs = [
				json[6].GetValue<double>() / 10000d,
				json[7].GetValue<double>() / 10000d,
				json[8].GetValue<double>() / 10000d,
				json[9].GetValue<double>() / 10000d,
				json[10].GetValue<double>() / 10000d,
				json[11].GetValue<double>() / 10000d,
				json[12].GetValue<double>() / 10000d,
				json[13].GetValue<double>() / 10000d,
				json[14].GetValue<double>() / 10000d,
			];
			
			double exp = Math.Exp(-t / T1);
			double yield = Bs[0] + (Bs[1] + Bs[2]) * (T1 / t) * (1 - exp) - Bs[2] * exp;
			double yieldFit = 0d;
			double a = 0d;
			double b = 0.6d;
			for (int i = 0; i < 9; i++)
			{
				yieldFit += Gs[i] * Math.Exp(-Math.Pow(t - a, 2) / Math.Pow(b, 2));
				a += 0.6 * Math.Pow(1.6, i);
				b *= 1.6;
			}
			yield += yieldFit; //This is the continuous (instantaneous) yield

			RiskFreeRate = Math.Exp(yield) - 1; //This is the conversion to annuall compounding
		}

*/