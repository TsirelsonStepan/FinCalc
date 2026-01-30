using FluentAssertions;

using FinCalc.Calculate;
using FinCalc.DataStructures;

namespace FinCalc.Tests;

public class HistoricTests
{
	readonly HistoricData TestHistoricPrices = new(
		"TestPrices",
		Frequency.Weekly,
		35, 
		[
			DateTime.Today,
			DateTime.Today.AddDays(-7),
			DateTime.Today.AddDays(-14),
			DateTime.Today.AddDays(-21),
			DateTime.Today.AddDays(-28),
			DateTime.Today.AddDays(-35)
		],
		[1d, 2d, 1d, 2d, 1d, 2d]);
	
	[Fact]
	public void Returns()
	{
		HistoricData result = Historic.Returns(TestHistoricPrices);

		result.Name.Should().Be(TestHistoricPrices.Name);
		result.Dates.Should().Equal(TestHistoricPrices.Dates[0..^1]);
		double?[] exp = [-0.5, 1, -0.5, 1, -0.5];
		result.Values.Should().Equal(exp);
		result.Frequency.Should().Be(TestHistoricPrices.Frequency);
		result.Period.Should().Be(TestHistoricPrices.Period - (int)TestHistoricPrices.Frequency);
	}

	[Fact]
	public void Total()
	{
		CustomContext context = new();
		HistoricData[] portfolioData = [TestHistoricPrices];
		double[] amounts = [1d];
		HistoricData result = Historic.Total(context, portfolioData, amounts);

		result.Name.Should().Be("Portfolio");
		result.Dates.Should().Equal(portfolioData[0].Dates);
		result.Values.Should().Equal(TestHistoricPrices.Values);
		result.Frequency.Should().Be(TestHistoricPrices.Frequency);
		result.Period.Should().Be(TestHistoricPrices.Period);
	}
}
