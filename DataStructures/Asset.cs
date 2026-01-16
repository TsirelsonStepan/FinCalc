namespace FinCalc.DataStructures
{
	public class Asset
	{
		public Asset() {}
		public string Secid { get; set; } = "";
		public string Shortname { get; set; } = "";
		public string Name { get; set; } = "";
		public string Description { get; set; } = "";

		public Asset(string _secid, string _shortname, string _name, string _description)
		{
			Secid  = _secid;
			Shortname  = _shortname;
			Name  = _name;
			Description  = _description;			
		}
	}

	public class AssetInPortfolio
	{
		public AssetInPortfolio() {}
		public string Secid { get; set; } = "";
		public string Market { get; set; } = "";
		public double Amount { get; set; } = 0;

		public AssetInPortfolio(string _market, string _secid, double _amount) : this()
		{
			Market = _market;
			Secid = _secid;
			Amount = _amount;			
		}
	}
}