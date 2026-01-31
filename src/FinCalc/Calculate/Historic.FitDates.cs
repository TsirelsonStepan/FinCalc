using FinCalc.DataStructures;

namespace FinCalc.Calculate;

public static partial class Historic
{
	//
	// Changes toFit.Dates to be identical to model.Dates, and recalcualtes values to correspond to new dates
	public static HistoricData FitDates(HistoricData toFit, IReadOnlyList<DateTime>? modelDates = null)
	{
		DateTime newStartDate = modelDates?[0] ?? toFit.Dates[0];
		DateTime newEndDate = modelDates?[0] ?? toFit.Dates[^1];
		if (DateTime.Compare(newEndDate, DateTime.Today) > 0) newEndDate = DateTime.Today;
		modelDates ??= GetStandardDates(toFit.Frequency, newStartDate, newEndDate);
		double?[] newValues = new double?[modelDates.Count];

		int skipped = 0;
		int i = 0;
		for (; i < modelDates.Count; i++)
		{
			if (i + skipped >= toFit.Dates.Count) break;
			int comp = DateTime.Compare(modelDates[i], toFit.Dates[i + skipped]);
			if (comp == 0) newValues[i] = toFit.Values[i + skipped];
			else if (comp > 0)
			{
				if (i + skipped >= toFit.Dates.Count - 1) newValues[i] = null;
				else if (DateTime.Compare(modelDates[i], toFit.Dates[i + 1 + skipped]) < 0 && DateTime.Compare(modelDates[i + 1], toFit.Dates[i + 1 + skipped]) > 0)
				{
					newValues[i] = GetAverageValue(
						toFit.Dates[i + skipped],
						toFit.Values[i + skipped],
						toFit.Dates[i + 1 + skipped],
						toFit.Values[i + 1 + skipped],
						modelDates[i]
					);
				}
				else
				{
					i--;
					skipped++;
				}
			}
			else if (comp < 0)
			{
				if (i + skipped <= 0)
				{
					skipped--;
					newValues[i] = null;
				}
				else if (DateTime.Compare(modelDates[i], toFit.Dates[i - 1 + skipped]) > 0 && DateTime.Compare(modelDates[i - 1], toFit.Dates[i - 1 + skipped]) < 0)
				{
					newValues[i] = GetAverageValue(
						toFit.Dates[i - 1 + skipped],
						toFit.Values[i - 1 + skipped],
						toFit.Dates[i + skipped],
						toFit.Values[i + skipped],
						modelDates[i]
					);
				}
				else
				{
					skipped--;
					newValues[i] = null;
				}
			}
			else throw new Exception("edge case");
		}
		for (; i < modelDates.Count; i++) newValues[i] = null; //if previous loop ended prematurely because data toFit ended, the rest of new values is filled wih nulls

		HistoricData result = new(toFit.Name, toFit.Frequency, modelDates, newValues);
		return result;
	}

	private static double? GetAverageValue(DateTime prevDate, double? prevValue, DateTime curDate, double? curValue, DateTime targetDate)
	{
		double shareOfCur = DateTime.Compare(curDate, targetDate);
		double shareOfPrev = DateTime.Compare(targetDate, prevDate);
		return (curValue * shareOfPrev + prevValue * shareOfCur) / (shareOfCur + shareOfPrev);
	}

	private static IReadOnlyList<DateTime> GetStandardDates(Frequency frequency, DateTime start, DateTime end)
	{
		List<DateTime> newDates = [start];
		DateTime lastAddedDate = start;
		while (DateTime.Compare(lastAddedDate, end) < 0)
		{
			if (frequency == Frequency.Daily) lastAddedDate = lastAddedDate.AddDays(1);
			else if (frequency == Frequency.Weekly) lastAddedDate = lastAddedDate.AddDays(7);
			else if (frequency == Frequency.Monthly)
			{
				lastAddedDate = lastAddedDate.AddMonths(1);
				lastAddedDate = new DateTime(
					lastAddedDate.Year,
					lastAddedDate.Month,
					(lastAddedDate.Day / (DateTime.DaysInMonth(lastAddedDate.Year, lastAddedDate.Month) - 1)) + 1//round either to 1 or last date of month
					);
				
			}
			else throw new NotImplementedException("No implementation for provided frequency");
			newDates.Add(lastAddedDate);
		}
		return newDates;
	}
}

			/*
			if (toFit.Dates[i].Date == modelDates[i].Date) newValues.Add(toFit.Values[i]);
			else if (DateTime.Compare(toFit.Dates[i].Date, modelDates[i].Date) < 0) //Average between toFit.Dates[i] and toFit.Dates[i + 1]
			else
			{
				if (DateTime.Compare(toFit.Dates[i].Date, modelDates[i + 1].Date) > 0) newValues.Add(null);
				else if (toFit.Dates[i].Date == modelDates[i + 1].Date) continue;
				else //Average between toFit.Dates[i - 1] and toFit.Dates[i]
			}*/