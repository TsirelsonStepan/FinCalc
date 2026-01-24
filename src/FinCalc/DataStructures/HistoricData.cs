namespace FinCalc.DataStructures;

public struct HistoricData
{
	public string Name { get; }
	public int[] Dates { get; } = [];
	public double?[] Values { get; } = [];
	public int Frequency { get; }
	public int Period { get; }

	public HistoricData(string name, int frequency, int period, int[] dates, double?[] values)
	{
		if (dates == null || dates.Length == 0 || values == null || values.Length == 0) throw new Exception("Attempt to copy zero array in constructor");
		
		Name = name;
		Frequency = frequency;
		Period = period;
		
		Dates = new int[dates.Length];
		Array.Copy(dates, Dates, dates.Length);
		Values = new double?[values.Length];
		Array.Copy(values, Values, values.Length);
	}

}