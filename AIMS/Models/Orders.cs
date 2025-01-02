using System.ComponentModel.DataAnnotations.Schema;

namespace AIMS.Models
{
    //public class Orders
    //{
    //    public int OrderId { get; set; }
    //    public string OrderNo { get; set; }
    //    public string ItemName { get; set; }
    //    public int OrderQuantity { get; set; }
    //    public string ItemSize { get; set; }
    //    public decimal ItemUnitPrice { get; set; }
    //    public decimal ItemTotalPrice { get; set; }
    //    public string CreatedAt { get; set; }
    //    public int ProductId { get; set; }
    //    public Product? Product { get; set; }
    //}

    //public class Orders
    //{
    //    public int OrderId { get; set; }
    //    public int OrderQuantity { get; set; }
    //    public decimal ItemTotalPrice { get; set; }
    //    public string CreatedAt { get; set; }
    //    public int ProductId { get; set; }
    //    public virtual Product Product { get; set; }
    //}

    //public class Orders
    //{
    //    public int OrderId { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //    public int ProductId { get; set; }

    //    public virtual Product Product { get; set; }
    //}

    public class Orders
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProductId { get; set; }
    }

    //public class Orders
    //{
    //    public int OrderId { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //    public ICollection<Product> Product { get; set; }
    //}

    //public class Orders
    //{
    //    public int OrderId { get; set; }
    //    public DateTime CreatedAt { get; set; }
    //    public int ProductId { get; set; }

    //    public Product Product { get; set; }
    //}
}
