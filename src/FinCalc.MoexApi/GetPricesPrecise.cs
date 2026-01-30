using FinCalc.MoexApi.Models;
using FinCalc.MoexApi.Enums;

namespace FinCalc.MoexApi;

public partial class MoexClient
{
	public async Task<IReadOnlyList<HistoricDataPoint>> GetPricesPrecise(SecurityInfo securityInfo, Frequency frequency, DateTime startDate)
	{
		IReadOnlyList<HistoricDataPoint> rawData = await GetHistoricDataRaw(securityInfo, frequency, startDate);
		List<HistoricDataPoint> result = [];
		DateTime currentDate = rawData[0].CloseDateTime;
		for (int i = 1; i < rawData.Count; i++)
		{
			if (rawData[i].CloseDateTime.Date == StepByFrequency(currentDate, frequency).Date)
			{
				
			}
		}
		return result;
	}

	private static DateTime StepByFrequency(DateTime dateTime, Frequency frequency)
	{
		if (frequency == Frequency.Daily) return dateTime.AddDays(1);
		else if (frequency == Frequency.Weekly) return dateTime.AddDays(7);
		else if (frequency == Frequency.Monthly)
		{
			DateTime d = dateTime.AddMonths(1);
			return new DateTime(d.Year, d.Month, DateTime.DaysInMonth(d.Year, d.Month));//MOEX return monthly data on last day of month
		}
		else throw new ArgumentException($"Unsupported frequency: {frequency}");		
	}
}