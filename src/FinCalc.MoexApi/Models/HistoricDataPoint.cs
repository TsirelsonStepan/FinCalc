namespace FinCalc.MoexApi.Models;

public class HistoricDataPoint
{
	public double OpenPrice { get; set; }
	public double ClosePrice { get; set; }
	public DateTime OpenDateTime { get; set; }
	public DateTime CloseDateTime { get; set; }
	
}