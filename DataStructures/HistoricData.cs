namespace FinCalc.DataStructures
{
	public class HistoricData
    {
		public HistoricData() {}
		
		public string Secid { get; set; } = "";
		public int[] Dates { get; set; } = [];
		public double?[] Values { get; set; } = [];

		public HistoricData(string name, int period)
		{
			Secid = name;
			Dates = new int[period];
			Values = new double?[period];
		}

		public HistoricData(string name, int period, int[] dates, double?[] values) : this(name, period)
		{
			if (dates == null || dates.Length == 0 || values == null || values.Length == 0) throw new Exception("Attempt to copy zero array in constructor");
			Array.Copy(dates, Dates, period);
			Array.Copy(values, Values, period);
		}
    }
}