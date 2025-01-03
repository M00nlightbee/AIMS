using System.ComponentModel.DataAnnotations.Schema;

namespace AIMS.Models
{
    public class Orders
    {
        public int OrderId { get; set; }
		public int OrderQuantity { get; set; }
		public DateTime CreatedAt { get; set; }
        public int ProductId { get; set; }
    }
}
