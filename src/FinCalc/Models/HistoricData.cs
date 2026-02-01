namespace FinCalc.Models;

public struct HistoricData
{
	public string Name { get; }
	public IReadOnlyList<DateTime> Dates { get; } = [];
	public IReadOnlyList<double?> Values { get; } = [];
	public Frequency Frequency { get; }

	public HistoricData(string name, Frequency frequency, IReadOnlyList<DateTime> dates, IReadOnlyList<double?> values)
	{
		Name = name;
		Frequency = frequency;
		
		Dates = dates;
		Values = values;
	}
}