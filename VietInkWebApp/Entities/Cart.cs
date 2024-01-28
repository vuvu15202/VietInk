using System.Text.Json.Serialization;

namespace VietInkWebApp.Entities
{
    public class Cart
    {
        public List<CartItem> CartItems { get; set; }
        //public int TotalPrice {
        //    get
        //    {
        //        if(CartItems == null) return 0;
        //        else return CartItems?.Sum(item => item.UnitPrice * item.Quantity) ?? 0;
        //    }
        //    set { TotalPrice = value; }
        //}

        [JsonIgnore]
        public int NumberOfItem { get { return CartItems == null ? 0: CartItems.Count; } set { NumberOfItem = value; } }

        public Cart()
        {
            CartItems = new List<CartItem>();
        }
    }
}
