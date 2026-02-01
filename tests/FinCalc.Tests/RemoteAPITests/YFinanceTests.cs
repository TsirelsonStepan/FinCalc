using FluentAssertions;

using FinCalc.Models;
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
		IReadOnlyList<Asset> result = await API.SecuritiesList("appl");
		result.Should().NotBeNull();
	}

	[Fact]
	public async Task CurrentPrice()
	{
		double result = await API.CurrentPrice(testAsset.Source.AssetPath);
		result.Should().BeGreaterThan(0);
	}
}