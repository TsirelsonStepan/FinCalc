namespace FinCalc.DataStructures;

public struct Asset
{
	public string Secid { get; set; } = "";
	public string Shortname { get; set; } = "";
	public string Name { get; set; } = "";
	public string Description { get; set; } = "";

	public Asset() {}

	public Asset(string secid, string shortname, string name, string description)
	{
		Secid  = secid;
		Shortname  = shortname;
		Name  = name;
		Description  = description;			
	}
}

public struct AssetInPortfolio
{
	public string Market { get; set; } = "";
	public string Secid { get; set; } = "";
	public double Amount { get; set; } = 0;

	public AssetInPortfolio() {}

	public AssetInPortfolio(string market, string secid, double amount)
	{
		Market = market;
		Secid = secid;
		Amount = amount;			
	}
}