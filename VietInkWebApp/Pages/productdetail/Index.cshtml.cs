using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VietInkWebApp.Entities;

namespace VietInkWebApp.Pages.productdetail
{
    public class IndexModel : PageModel
    {
        private readonly TattooshopContext _context;
        private readonly IConfiguration Configuration;
        private IHttpContextAccessor session;


        public IndexModel(TattooshopContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;

        }

        public Product Product { get; set; } = default!;
        


        //public async Task<IActionResult> OnGetAsync(int? id, int? quantity)
        //{
        //    if (id == null || _context.Products == null)
        //    {
        //        return NotFound();
        //    }

        //    if (quantity != null && id != null)
        //    {
        //        Quantity = (int)quantity;
        //        ProductId = (int)id;
        //    }

        //    var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        Product = product;
        //    }
        //    return Page();
        //}
        public async Task OnGetAsync(int id)
        {
            if (id == null || _context.Products == null)
            {
            }


            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
            }
            else
            {
                Product = product;
            }
        }


        public Cart Cart { get; set; }
        [BindProperty]
        public int Quantity { get; set; } =1;
        [BindProperty]
        public int ProductId { get; set; } = default!;
        [BindProperty]
        public int UnitPrice { get; set; } = default!;
        public IActionResult OnPostAsync()
        {
            //var quan = Int32.Parse(Request.Form["Quantity"].ToString());
            //var proid = Int32.Parse(Request.Form["ProductId"].ToString());

            getCartCookies();
            
            if (Cart.CartItems != null)
            {
                CartItem cartitem = Cart.CartItems.SingleOrDefault(p => p.ProductId == ProductId);
                if (cartitem != null)
                {
                    foreach (var item in Cart.CartItems.Where(x => x.ProductId == ProductId))
                    {
                        item.Quantity = Quantity;
                    }
                }
                else
                {
                    Cart.CartItems.Add(new CartItem { ProductId = ProductId, Quantity = Quantity, UnitPrice = UnitPrice });
                }
            }
            else
            {
                Cart.CartItems = new List<CartItem>();
                Cart.CartItems.Add(new CartItem { ProductId = ProductId, Quantity = Quantity, UnitPrice = UnitPrice });
            }
            
            setCartCookies();

            return RedirectToPage("/shoppingcart/Index");
           
        }



        public void setCartCookies()
        {
            var cookieOptions1 = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1) // Set in the past
            };  Response.Cookies.Append("Cart", "", cookieOptions1);


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
                }else Cart = new Cart();
            }
            catch(Exception ex)
            {
                Cart = new Cart();
            }
            
        }

    }
}
