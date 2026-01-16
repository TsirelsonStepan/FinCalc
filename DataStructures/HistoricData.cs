namespace FinCalc.DataStructures
{
	public class HistoricData
    {
		public HistoricData() {}
		
		public string Name { get; set; } = "";
		public int[] Dates { get; set; } = [];
		public double?[] Values { get; set; } = [];
		public int Frequency { get; set; } = 7;
		public int Period { get; set; } = 365; //real time period in days

		public HistoricData(string name, int length, int freq, int period)
		{
			Name = name;
			Dates = new int[length];
			Values = new double?[length];
			Frequency = freq;
			Period = period;
		}

		public HistoricData(string name, int length, int freq, int period, int[] dates, double?[] values) : this(name, length, freq, period)
		{
			if (dates == null || dates.Length == 0 || values == null || values.Length == 0) throw new Exception("Attempt to copy zero array in constructor");
			Array.Copy(dates, Dates, length);
			Array.Copy(values, Values, length);
		}
    }
}