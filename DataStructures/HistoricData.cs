namespace FinCalc.DataStructures
{
	public class HistoricData
    {
		public HistoricData() {}
		
		public string Name { get; set; } = "";
		public string[] Dates { get; set; } = [];
		public double[] Values { get; set; } = [];
		public int Interval { get; set; } = 0;
		public int Length { get; set; } = 0;

		public HistoricData(string name, int length, int interval)
		{
			Name = name;
			Interval = interval; //in days
			Length = length;
			Dates = new string[length];
			Values = new double[length];
		}

		public HistoricData(string name, int length, int interval, string[] dates, double[] values) : this(name, length, interval)
		{
			if (dates == null || dates.Length == 0 || values == null || values.Length == 0) throw new Exception("Attempt to copy zero array in constructor");
			dates.CopyTo(Dates, 0);
			values.CopyTo(Values, 0);
		}

		public HistoricData FillMissing(string[] newDates)
		{
			HistoricData newData = new(Name, newDates.Length, Interval);
			if (newDates.Length <= Length) return this;
			double lastValue = Values[0];
			int added_values = 0;
			for (int i = 0; i < newDates.Length; i++)
			{
				newData.Dates[i - added_values] = newDates[i];
				if (Dates[i - added_values] == newDates[i])
				{
					newData.Values[i] = Values[i];
					lastValue = Values[i];
				}
				else
				{
					newData.Values[i] = lastValue;
					added_values++;
				}
			}
			return newData;
		}
    }
}