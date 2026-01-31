using System.ComponentModel.DataAnnotations;
using FinCalc.DataStructures;

public struct HistoricDataResponce
{
	[Required]
	public IReadOnlyList<string> Dates { get; set; } = [];

	[Required]
	public IReadOnlyList<double?> Values { get; set; } = [];

	public HistoricDataResponce() {}

	public HistoricDataResponce(HistoricData data)
	{
		string[] newDates = new string[data.Dates.Count];
		for (int i = 0; i < data.Dates.Count; i++)
			newDates[i] = $"{data.Dates[i]:yyyy-MM-dd}";
		Dates = newDates;
		Values = data.Values;
	} 
}