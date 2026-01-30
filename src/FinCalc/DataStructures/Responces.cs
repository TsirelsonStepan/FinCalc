using System.ComponentModel.DataAnnotations;
using FinCalc.DataStructures;

public struct HistoricDataResponce
{
	[Required]
	public string[] Dates { get; set; } = [];

	[Required]
	public double?[] Values { get; set; } = [];

	public HistoricDataResponce() {}

	public HistoricDataResponce(HistoricData data)
	{
		Dates = new string[data.Dates.Length];
		for (int i = 0; i < data.Dates.Length; i++)
			Dates[i] = $"{data.Dates[i]:yyyy-MM-dd}";
		Values = data.Values;
	} 
}