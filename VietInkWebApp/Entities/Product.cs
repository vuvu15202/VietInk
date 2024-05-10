using System;
using System.Collections.Generic;

namespace VietInkWebApp.Entities
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? CategoryName { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? UnitsInStock { get; set; }
        public string? Image { get; set; }
        public bool Discontinued { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}










































