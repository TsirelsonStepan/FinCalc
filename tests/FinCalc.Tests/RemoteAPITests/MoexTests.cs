using FluentAssertions;

using FinCalc.Models;
using FinCalc.RemoteAPIs;

namespace FinCalc.Tests;

public class MoexTests
{
	readonly IRemoteAPI API = new MoexAPI();

	Asset testAsset = new()
    {
		Source = new()
		{
			Api = "moex",
			AssetPath = "stock/shares/TQBR/SBER"
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

    [Fact]
    public async Task Prices()
    {
        CustomContext context = new();
        HistoricData result = await API.Prices(context, testAsset.Source.AssetPath, Frequency.Weekly, 365);
        result.Should().NotBeNull();
    }
}