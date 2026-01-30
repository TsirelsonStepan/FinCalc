using FinCalc.DataStructures;

using System.ComponentModel.DataAnnotations;

//
// the pair of frequency (as 1 observation value per frequency in days) and period (as number of days)
public record TimeSeriesRequest
{
	//
	// the frequency of observations 1 value per frequency in days (accept only 1, 7, 31, 90)
	[Required]
	public Frequency? Frequency { get; set; }

	//
	// the time period in days (limited to values between 90 days and 5 years (365 * 5 days))
	[Required][Range(90, 365 * 5)]
	public int? Period { get; set; }

	public TimeSeriesRequest() {}

	public TimeSeriesRequest(Frequency frequency, int period)
	{
		Frequency = frequency;
		Period = period;
	}
}

//
// the DTO for accepting http requests for histroic data of asset
public record HistoricDataRequest
{
	//
	// the SECurityID (for now only for MOEX API)
	[Required]
	public Source Source { get; set; } = new();

	//
	//
	[Required]
	public TimeSeriesRequest? TimeSeries { get; set; }

	public HistoricDataRequest() {}
}

public record CAPMRequest
{
	[Required]
	public AssetInPortfolio[]? Assets { get; set; }

	[Required]
	public HistoricDataRequest? Benchmark { get; set; }

	public CAPMRequest() {}
}