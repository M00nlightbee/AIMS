namespace AIMS.Models
{
	public class Analytics
	{
		public int TotalQuantity { get; set; }
		public Dictionary<string, int> StockByBranch { get; set; } = new Dictionary<string, int>();
		public Dictionary<string, int> StockByCategory { get; set; } = new Dictionary<string, int>();
	}
}
