namespace FinCalc.DataStructures
{
	public class HistoricData
    {
		public HistoricData() {}
		
		public string Secid { get; set; } = "";
		public int[] Dates { get; set; } = [];
		public double[] Values { get; set; } = [];

		public HistoricData(string name, int periods)
		{
			Secid = name;
			Dates = new int[periods];
			Values = new double[periods];
		}

		public HistoricData(string name, int periods, string[] dates, double[] values) : this(name, periods)
		{
			if (dates == null || dates.Length == 0 || values == null || values.Length == 0) throw new Exception("Attempt to copy zero array in constructor");
			dates.CopyTo(Dates, 0);
			values.CopyTo(Values, 0);
		}
    }
}