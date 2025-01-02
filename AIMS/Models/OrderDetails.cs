using AIMS.Data;

namespace AIMS.Models
{
    public class OrderDetails
    {
        //public Product product { get; set; }
        //public Orders orders { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSize { get; set; }
        public decimal Price { get; set; }

    }
}
