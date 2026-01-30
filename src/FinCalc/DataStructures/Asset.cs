namespace FinCalc.DataStructures;

public record Asset
{
	public Source Source { get; set; } = new();
	public string? Shortname { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }

	public Asset() {}
}

public record AssetInPortfolio
{
	public Source Source { get; set; } = new();
	public double Amount { get; set; }

	public AssetInPortfolio() {}
}