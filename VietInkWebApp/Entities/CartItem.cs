using System.Text.Json.Serialization;

namespace VietInkWebApp.Entities
{
    public class CartItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public int UnitPrice { get; set; }

        [JsonIgnore]
        public int? TotalPrice { get { return Quantity * UnitPrice; } set { TotalPrice = value; } }

        [JsonIgnore]
        public Product Product { get; set; } = new Product();

    }
}
