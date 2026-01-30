using FluentAssertions;

using FinCalc.DataStructures;
using FinCalc.RemoteAPIs;

namespace FinCalc.Tests;

public class YFinanceTests
{
	readonly IRemoteAPI API = new YFinanceAPI();

	Asset testAsset = new()
    {
		Source = new()
		{
			Api = "yfinance",
			AssetPath = "AAPL",
		},
		Name = "Apple Inc."
	};

	[Fact]
	public async Task SecuritiesList()
	{
		Asset[] result = await API.SecuritiesList("appl");
		result.Should().NotBeNull();
	}

	[Fact]
	public async Task CurrentPrice()
	{
		double result = await API.CurrentPrice(testAsset.Source.AssetPath);
		result.Should().BeGreaterThan(0);
	}
}