using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Text.Json;
using VietInkWebApp.Entities;

namespace VietInkWebApp.Pages.checkout
{
    public class IndexModel : PageModel
    {
        private readonly VietInkWebApp.Entities.TattooshopContext _context;

        public IndexModel(VietInkWebApp.Entities.TattooshopContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            getCartCookies();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;
        [BindProperty]
        public User User { get; set; } = default!;
        [BindProperty]
        public List<OrderDetail> OrderDetails { get; set; } = default!;

        public Cart Cart { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            getCartCookies();

            if (!ModelState.IsValid )
            {

                return Page();
            }



            try
            {

                User.UserName = RandomStringGenerator.GenerateRandomString(10);
                User.Role = 3;
                User.Password ="000000";
                _context.Users.Add(User);
                await _context.SaveChangesAsync();

                Order.UserId = User.UserId;
                Order.OrderDate = DateTime.Now;
                Order.ShipCountry = "Việt Nam";
                Order.Note = "22000 ship";

                _context.Orders.Add(Order);
                await _context.SaveChangesAsync();


                var orderDetails = new List<OrderDetail>();
                foreach(var item in Cart.CartItems){
                    orderDetails.Add(
                        new OrderDetail { 
                            OrderId = Order.OrderId,
                            ProductId = item.ProductId,
                            UnitPrice = item.UnitPrice,
                            Quantity = item.Quantity
                        });
                }
                _context.OrderDetails.AddRange(orderDetails);
                await _context.SaveChangesAsync();
                setCartCookies();
            }catch(Exception ex)
            {

            }
            return RedirectToPage("/shoppingcart/Index");
        }

        public void setCartCookies()
        {
            Cart.CartItems.Clear();
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
                    foreach (var item in Cart.CartItems)
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
        public class RandomStringGenerator
        {
            private static Random random = new Random();

            public static string GenerateRandomString(int length)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
    }
}
