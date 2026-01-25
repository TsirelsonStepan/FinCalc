using FinCalc.DataStructures;

using System.ComponentModel.DataAnnotations;

//
// the pair of frequency (as 1 observation value per frequency in days) and period (as number of days)
public record TimeSeriesRequest
{
	//
	// the frequency of observations 1 value per frequency in days (accept only 1, 7, 31, 90)
	[Required][AllowedValues(1, 7, 30, 90, "1", "7", "30", "90")]
	public int? Frequency { get; set; }

	//
	// the time period in days (limited to values between 90 days and 5 years (365 * 5 days))
	[Required][Range(90, 365 * 5)]
	public int? Period { get; set; }

	public TimeSeriesRequest() {}

	public TimeSeriesRequest(int frequency, int period)
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
	// the Source security/asset is traded on - combination of API and market(only for MOEX API)
	[Required]
	public Source? Source { get; set; }

	//
	// the SECurityID (for now only for MOEX API)
	[Required][MinLength(2)]
	public string? Secid { get; set; }

	//
	//
	[Required]
	public TimeSeriesRequest? TimeSeries { get; set; }

	public HistoricDataRequest() {}
}

public record Source
{
	[Required] [AllowedValues("MOEX", "AlphaVantage")]
	public string? Api { get; set; }

	[AllowedValues("shares", "index")]
	public string? Market { get; set; }

	public Source() {}

	public Source(string api, string market)
	{
		Api = api;
		Market = market;
	}
}

public record CAPMRequest
{
	[Required]
	public AssetInPortfolio[]? Assets { get; set; }

	[Required]
	public HistoricDataRequest? Benchmark { get; set; }

	public CAPMRequest() {}
}