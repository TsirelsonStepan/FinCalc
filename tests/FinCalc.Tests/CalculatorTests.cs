using FluentAssertions;

using FinCalc.Calculate;
using FinCalc.DataStructures;

namespace FinCalc.Tests;

public class CalculatorTests
{
    readonly HistoricData TestHistoricPrices = new("TestPrices", 7, 35, [0, 7, 14, 21, 28, 35], [1d, 2d, 1d, 2d, 1d, 2d]);

    readonly HistoricData TestHistoricReturns = new("TestReturns", 7, 35, [0, 7, 14, 21, 28, 35], [0.01, 0.01, 0.01, 0.01, 0.01, 0.01]);

	[Fact]
	public void AnnualReturns()
	{
		double result = Indicator.AnnualReturn(TestHistoricReturns);
		result.Should().BeApproximately(0.52, 0.01);
	}

	[Fact]
	public void Beta()
	{

	}
	
	[Fact]
	public void Returns()
	{
		HistoricData result = Historic.Returns(TestHistoricPrices);
		result.Name.Should().Be(TestHistoricPrices.Name);

		for (int i = 0; i < 5; i++) result.Dates[i].Should().Be(TestHistoricPrices.Dates[i]);

		double[] exp = [-0.5, 1, -0.5, 1, -0.5];
		for (int i = 0; i < 5; i++) result.Values[i].Should().Be(exp[i]);
		result.Frequency.Should().Be(TestHistoricPrices.Frequency);
		result.Period.Should().Be(TestHistoricPrices.Period - TestHistoricPrices.Frequency);
	}
}
