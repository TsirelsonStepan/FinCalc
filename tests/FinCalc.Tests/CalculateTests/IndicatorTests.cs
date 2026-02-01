using FluentAssertions;

using FinCalc.Calculate;
using FinCalc.Models;

namespace FinCalc.Tests;

public class CalculateTests
{
	readonly HistoricData TestHistoricReturns = new(
		"TestReturns",
		Frequency.Weekly,
		[
			DateTime.Today.AddDays(-35),
			DateTime.Today.AddDays(-28),
			DateTime.Today.AddDays(-21),
			DateTime.Today.AddDays(-14),
			DateTime.Today.AddDays(-7),
			DateTime.Today
		],
		[0.01, 0.02, 0.03, 0.02, 0.01, 0.02]);

	readonly HistoricData TestHistoricBenchmarkReturns = new(
		"TestBenchmarkReturns",
		Frequency.Weekly,
		[
			DateTime.Today.AddDays(-35),
			DateTime.Today.AddDays(-28),
			DateTime.Today.AddDays(-21),
			DateTime.Today.AddDays(-14),
			DateTime.Today.AddDays(-7),
			DateTime.Today
		],
		[0.01, 0.02, 0.03, 0.02, 0.01, 0.02]);

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
