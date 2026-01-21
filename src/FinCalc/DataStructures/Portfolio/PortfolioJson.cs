using System.Text.Json;

namespace FinCalc.DataStructures
{	
	public partial class Portfolio
	{
        static readonly JsonSerializerOptions options = new()
        {
			PropertyNameCaseInsensitive = true
		};
		public static Portfolio Deserialize(string portfolioJson)
		{
			return JsonSerializer.Deserialize<Portfolio>(portfolioJson, options) ?? throw new PortfolioSizeIsZero("portfolio json deserializaion failed");
		}
		public static string Serialize(Portfolio portfolio)
		{
			return JsonSerializer.Serialize(portfolio, options);
		}
	}
}