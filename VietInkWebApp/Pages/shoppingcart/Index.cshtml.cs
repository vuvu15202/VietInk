using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VietInkWebApp.Entities;

namespace VietInkWebApp.Pages.shoppingcart
{
    public class IndexModel : PageModel
    {
        private readonly TattooshopContext _context;
        private readonly IConfiguration Configuration;


        public IndexModel(TattooshopContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;

        }

        public Cart Cart { get; set; }

        [BindProperty]
        public int Status { get; set; } = 0;
        //[BindProperty]
        //public int Quantity { get; set; } 
        //[BindProperty]
        //public int ProductId { get; set; } = default!;
        //[BindProperty]
        //public int UnitPrice { get; set; } = default!;
        public async Task OnGetAsync(int? quantity, int? productId, int? unitPrice)
        {
            getCartCookies();

            if (quantity != null && productId!= null && unitPrice != null)
            {
                
                if (Cart.CartItems != null)
                {
                    CartItem cartitem = Cart.CartItems.SingleOrDefault(p => p.ProductId == productId);
                    if (cartitem != null)
                    {
                        if (quantity == 0)
                        {
                            Cart.CartItems.RemoveAll(item => item.ProductId == productId);
                        }
                        else
                        {
                            if (_context.Products.SingleOrDefault(p => p.ProductId == productId).UnitsInStock >= quantity)
                            {
                                foreach (var item in Cart.CartItems.Where(x => x.ProductId == productId))
                                {
                                    item.Quantity = (int)quantity;
                                    Status = 1;
                                }
                            }
                            else if(_context.Products.SingleOrDefault(p => p.ProductId == productId).UnitsInStock == 0)
                            {
                                Cart.CartItems.RemoveAll(item => item.ProductId == productId);
                                Status = 3;
                            }
                            else
                            {
                                foreach (var item in Cart.CartItems.Where(x => x.ProductId == productId))
                                {
                                    item.Quantity = (int)_context.Products.SingleOrDefault(p => p.ProductId == productId).UnitsInStock;
                                }
                                Status = 2;
                            }
                            
                        }
                        
                    }
                    else
                    {
                        Cart.CartItems.Add(new CartItem { ProductId = (int)productId, Quantity = (int)quantity, UnitPrice = (int)unitPrice });
                    }
                }
                else
                {
                    Cart.CartItems = new List<CartItem>();
                    Cart.CartItems.Add(new CartItem { ProductId = (int)productId, Quantity = (int)quantity, UnitPrice = (int)unitPrice });
                }

                setCartCookies();
            }
            else
            {
                getCartCookies();

            }
        }


        public void setCartCookies()
        {
            var cookieOptions1 = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1) // Set in the past
            }; Response.Cookies.Append("Cart", "", cookieOptions1);


            var cookieOptions2 = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1), // Set the expiry date
                HttpOnly = true, // A security measure
                Secure = true // Send the cookie over HTTPS only
            };

            JsonSerializerOptions options = new JsonSerializerOptions();
            //format dep
            options.WriteIndented = true;
            string jsonData = JsonSerializer.Serialize(Cart, options);

            Response.Cookies.Append("Cart", jsonData, cookieOptions2);

        }

        public void getCartCookies()
        {
            try
            {
                string jsonData = string.IsNullOrEmpty(Request.Cookies["Cart"]) ? "" : Request.Cookies["Cart"];
                if (!jsonData.Equals(""))
                {
                    Cart = JsonSerializer.Deserialize<Cart>(jsonData);
                    foreach(var item in Cart.CartItems)
                    {
                        item.Product = _context.Products.SingleOrDefault(ci => ci.ProductId == item.ProductId);
                    }
                }
                else Cart = new Cart();
            }
            catch (Exception ex)
            {
                Cart = new Cart();
            }

        }

    }
}
