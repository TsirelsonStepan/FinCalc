using System.ComponentModel.DataAnnotations;

namespace FinCalc.Models.DTOs;

public struct HistoricDataResponse
{
	[Required]
	public IReadOnlyList<string> Dates { get; set; } = [];

	[Required]
	public IReadOnlyList<double?> Values { get; set; } = [];

	public HistoricDataResponse() {}

	public HistoricDataResponse(HistoricData data)
	{
		string[] newDates = new string[data.Dates.Count];
		for (int i = 0; i < data.Dates.Count; i++)
			newDates[i] = $"{data.Dates[i]:yyyy-MM-dd}";
		Dates = newDates;
		Values = data.Values;
	} 
}