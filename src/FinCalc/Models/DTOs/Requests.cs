using System.ComponentModel.DataAnnotations;

namespace FinCalc.Models.DTOs;

public record TimeSeriesRequest
{
	[Required]
	public Frequency? Frequency { get; set; }

	[Required]
	public int? Period { get; set; }

	public TimeSeriesRequest() {}
}

public record HistoricDataRequest
{
	[Required]
	public Source Source { get; set; } = new();

	[Required]
	public TimeSeriesRequest? TimeSeries { get; set; }

	public HistoricDataRequest() {}
}

public record CAPMRequest
{
	[Required]
	public IReadOnlyList<AssetInPortfolio>? Assets { get; set; }

	[Required]
	public HistoricDataRequest? Benchmark { get; set; }

	public CAPMRequest() {}
}