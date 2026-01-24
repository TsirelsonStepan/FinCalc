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
        Dates = GetRealDates(data.Dates);
        Values = data.Values;
    }

	private readonly string[] GetRealDates(int[] pastDays)
	{
		DateTime today = DateTime.Today;
        string[] realDates = new string[pastDays.Length];
		for (int i = 0; i < pastDays.Length; i++)
		{
            realDates[i] = $"{today.AddDays(-pastDays[i]):yyyy-MM-dd}";
		}
		return realDates;
	}    
}