using AIMS.Data;

namespace AIMS.Models
{
	//model for joining tables 
	public class OrderDetails
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSize { get; set; }
        public decimal Price { get; set; }
		public int OrderQuantity { get; set; }
        public int OrderNumber { get; set; }
		public decimal ItemTotalPrice { get; set; }
		public int ArchivedOrderId { get; set; }
		public DateTime CreatedAt { get; set; }

	}
}
