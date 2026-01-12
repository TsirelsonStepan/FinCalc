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
		public Asset Asset { get; set; } = null!;
		public double Amount { get; set; } = 0;

		public AssetInPortfolio(Asset _asset, double _amount) : this()
		{
			Asset = _asset;
			Amount = _amount;			
		}
	}
}