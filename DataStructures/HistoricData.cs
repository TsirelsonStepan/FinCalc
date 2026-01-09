namespace FinCalc.DataStructures
{
	public readonly struct HistoricData()
    {
		public string Name { get; } = "";
		public string[] Dates { get; } = [];
		public double[] Values { get; } = [];
		public int Interval { get; }
		public int Length { get; }
		public HistoricData(string name, int length, int interval) : this()
		{
			Name = name;
			Interval = interval; //in days
			Length = length;
			Dates = new string[length];
			Values = new double[length];
		}

		public HistoricData(string name, int length, int interval, string[] dates, double[] values) : this()
		{
			if (dates == null || dates.Length == 0 || values == null || values.Length == 0) throw new Exception("Attempt to copy zero array in constructor");
			Name = name;
			Interval = interval; //in days
			Length = length;
			Dates = new string[length];
			Values = new double[length];
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