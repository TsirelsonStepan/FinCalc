using FluentAssertions;

using FinCalc.Calculate;
using FinCalc.DataStructures;

namespace FinCalc.Tests;

public class CalculateTests
{
    readonly HistoricData TestHistoricReturns = new("TestReturns", 7, 35, [0, 7, 14, 21, 28, 35], [0.01, 0.02, 0.03, 0.02, 0.01, 0.02]);

    readonly HistoricData TestHistoricBenchmarkReturns = new("TestBenchmarkReturns", 7, 35, [0, 7, 14, 21, 28, 35], [0.01, 0.02, 0.03, 0.02, 0.01, 0.02]);

	[Fact]
	public void AnnualReturns()
	{
		double result = Indicator.AnnualReturn(TestHistoricReturns);
		result.Should().BeApproximately(0.952128854, 0.01);
	}

	[Fact]
	public void Beta()
	{
		double result = Indicator.Beta(TestHistoricReturns, TestHistoricBenchmarkReturns);
		result.Should().BeApproximately(1, 0.01);
	}
}
