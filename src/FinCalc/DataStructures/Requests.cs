using System.ComponentModel.DataAnnotations;

//
// the pair of frequency (as 1 observation value per frequency in days) and period (as number of days)
public record TimeSeriesRequest
{
	//
	// the frequency of observations 1 value per frequency in days (accept only 1, 7, 31, 90)
	[Required][AllowedValues(1, 7, 30, 90, "1", "7", "30", "90")]
	public int Frequency { get; set; }

	//
	// the time period in days (limited to values between 90 days and 5 years (365 * 5 days))
	[Required][Range(90, 365 * 5)]
	public int Period { get; set; }

	public static TimeSeriesRequest Default { get; } = new(7, 365);

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
	// the market security/asset is traded on (for now only for MOEX API)
	[Required][MinLength(2)]
	public string? Market { get; set; }

	//
	// the SECurityID (for now only for MOEX API)
	[Required][MinLength(2)]
	public string? Secid { get; set; }

	//
	//
	public TimeSeriesRequest TimeSeries { get; set; } = TimeSeriesRequest.Default;

	public HistoricDataRequest() {}
}