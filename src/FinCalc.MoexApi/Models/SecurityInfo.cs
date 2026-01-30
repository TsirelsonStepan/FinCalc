namespace FinCalc.MoexApi.Models;

public class SecurityInfo
{
	public string? Engine { get; set; }
	public string? Board { get; set; }
	public string? Market { get; set; }
	public string Secid { get; set; } = "";

	public string? Shortname { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
}