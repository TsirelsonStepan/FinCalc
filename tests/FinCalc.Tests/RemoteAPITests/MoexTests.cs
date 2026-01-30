using FluentAssertions;

using FinCalc.DataStructures;
using FinCalc.RemoteAPIs;

namespace FinCalc.Tests;

public class MoexTests
{
	readonly IRemoteAPI API = new MoexAPI();

	Asset testAsset = new()
    {
		AssetPath = "stock/shares/TQBR/SBER",
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
		double result = await API.CurrentPrice(testAsset.AssetPath);
		result.Should().BeGreaterThan(0);
	}

    [Fact]
    public async Task Prices()
    {
        CustomContext context = new();
        HistoricData result = await API.Prices(context, testAsset.AssetPath, Frequency.Weekly, 365);
        result.Should().NotBeNull();
    }
}