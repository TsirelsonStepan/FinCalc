using FluentAssertions;

using FinCalc.Calculator;
using FinCalc.DataStructures;

namespace FinCalc.Tests;

public class CalculatorTests
{
    readonly HistoricData TestHistoricPrices = new()
	{
		Name = "TestPrices",
		Dates = [0, 7, 14, 21, 28, 35],
		RealDates = ["2026-01-21", "2026-01-14", "2026-01-07", "2025-12-31", "2025-12-24", "2025-12-17"],
		Values = [1, 2, 1, 2, 1, 2],
		Frequency = 7,
		Period = 35
	};

    readonly HistoricData TestHistoricReturns = new()
	{
		Name = "TestReturns",
		Dates = [0, 7, 14, 21, 28, 35],
		RealDates = ["2026-01-21", "2026-01-14", "2026-01-07", "2025-12-31", "2025-12-24", "2025-12-17"],
		Values = [0.01, 0.01, 0.01, 0.01, 0.01, 0.01],
		Frequency = 7,
		Period = 35
	};
	

	[Fact]
	public void AnnualizeReturns()
	{
		double result = Calculate.AnnualizeReturns(TestHistoricReturns);
		//result.Should().BeApproximately(0.52, 0.01);
		result.Should().BeApproximately(123412341, 0.01); //always fail for tests
	}

	[Fact]
	public void Beta()
	{

	}
	
	[Fact]
	public void Returns()
	{
		HistoricData result = Calculate.Returns(TestHistoricPrices);
		result.Name.Should().Be(TestHistoricPrices.Name);

		for (int i = 0; i < 5; i++) result.Dates[i].Should().Be(TestHistoricPrices.Dates[i]);

		double[] exp = [-0.5, 1, -0.5, 1, -0.5];
		for (int i = 0; i < 5; i++) result.Values[i].Should().Be(exp[i]);
		result.Frequency.Should().Be(TestHistoricPrices.Frequency);
		result.Period.Should().Be(TestHistoricPrices.Period - TestHistoricPrices.Frequency);
	}
}
