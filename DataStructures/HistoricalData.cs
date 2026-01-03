namespace FinCalc.DataStructures
{
	public readonly struct HistoricalData(string name, int length, int interval)
    {
		public string Name { get; } = name;
		public string[] Dates { get; } = new string[length];
		public double[] Values { get; } = new double[length];
		public int Interval { get; } = interval; //in days
		public int Length { get; } = length;

		public HistoricalData FillMissing(string[] newDates)
		{
			HistoricalData newData = new(Name, newDates.Length, Interval);
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