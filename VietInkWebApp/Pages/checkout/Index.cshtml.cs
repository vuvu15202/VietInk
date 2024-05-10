using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Web;
using VietInkWebApp.Entities;

namespace VietInkWebApp.Pages.checkout
{
    public class IndexModel : PageModel
    {
        private readonly VietInkWebApp.Entities.TattooshopContext _context;
        private readonly IEmailSender _emailSender;

        public IndexModel(VietInkWebApp.Entities.TattooshopContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task OnGetAsync()
        {
            //if (status != null) Status = (int)status;
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
        [BindProperty]
        public int Status { get; set; } = 0;

        //Task<IActionResult>
        public async Task OnPostAsync()
        {
            getCartCookies();

            if (!ModelState.IsValid )
            {

                //return Page();
                return;
            }

            if (checkUnitInStock() && Cart.CartItems.Count != 0)
            {
                try
                {

                    User.UserName = RandomStringGenerator.GenerateRandomString(10);
                    User.Role = 3;
                    User.Password = "000000";
                    _context.Users.Add(User);
                    await _context.SaveChangesAsync();


                    Order.UserId = User.UserId;
                    Order.OrderDate = DateTime.Now;
                    Order.ShipCountry = "Việt Nam";

                    if (Cart.CartItems.Sum(cartItem => cartItem.UnitPrice * cartItem.Quantity) < 100000)
                    {
                        Order.Note = "22000 ship";
                        Order.Freight = Cart.CartItems.Sum(cartItem => cartItem.UnitPrice * cartItem.Quantity) + 22000;
                    }else
                    {
                        Order.Note = "free ship";
                        Order.Freight = Cart.CartItems.Sum(cartItem => cartItem.UnitPrice * cartItem.Quantity);
                    }


                    _context.Orders.Add(Order);
                    await _context.SaveChangesAsync();


                    var orderDetails = new List<OrderDetail>();
                    foreach (var item in Cart.CartItems)
                    {
                        orderDetails.Add(
                            new OrderDetail
                            {
                                OrderId = Order.OrderId,
                                ProductId = item.ProductId,
                                UnitPrice = item.UnitPrice,
                                Quantity = item.Quantity
                            });
                        Product updateProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);
                        if (updateProduct != null)
                        {
                            updateProduct.UnitsInStock = updateProduct.UnitsInStock - item.Quantity;
                            _context.Products.Update(updateProduct);
                        }
                    }
                    OrderDetails.AddRange(orderDetails);
                    _context.OrderDetails.AddRange(orderDetails);

                    await _context.SaveChangesAsync();
                    setCartCookies();


                    //send mail
                    Thread thread = new Thread(() => {
                        _emailSender.SendEmailAsync(User.Email, "Đặt hàng thành công", EmailHTML()).Wait();
                    });
                    thread.Start();

                }
                catch (Exception ex)
                {
                    Status = 2;

                }
                Status = 1;
            }
            else { Status = 2; }

            //return RedirectToPage("/shoppingcart/Index");
            //return RedirectToPage("./Index");

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

        public bool checkUnitInStock()
        {
            bool check = true;
            foreach(var item in Cart.CartItems)
            {
                var product = _context.Products.SingleOrDefault(p => p.ProductId == item.ProductId);
                if(product != null) {
                    if (product.UnitsInStock < item.Quantity) check = false;
                }
            }
            return check;
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

        public string EmailHTML()
        {
            string html = $"<div>\r\n  <table align='center' background='https://s3.amazonaws.com/swu-filepicker/4E687TRe69Ld95IDWyEg_bg_top_02.jpg'\r\n    border='0' cellpadding='0' cellspacing='0' role='presentation'\r\n    style='background:url(https://s3.amazonaws.com/swu-filepicker/4E687TRe69Ld95IDWyEg_bg_top_02.jpg) top center / auto repeat;width:100%;'>\r\n    <tbody>\r\n      <tr>\r\n        <td>\r\n          <!--[if mso | IE]>\r\n<v:rect style=\"mso-width-percent:1000;\" xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"true\" stroke=\"false\"><v:fill src=\"https://s3.amazonaws.com/swu-filepicker/4E687TRe69Ld95IDWyEg_bg_top_02.jpg\" origin=\"0.5, 0\" position=\"0.5, 0\" type=\"tile\" /><v:textbox style=\"mso-fit-shape-to-text:true\" inset=\"0,0,0,0\">\r\n<![endif]-->\r\n          <div style='margin:0px auto;max-width:600px;'>\r\n            <div style='font-size:0;line-height:0;'>\r\n              <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%;'>\r\n                <tbody>\r\n                  <tr>\r\n                    <td\r\n                      style='direction:ltr;font-size:0px;padding:20px 0px 30px 0px;text-align:center;vertical-align:top;'>\r\n                      <!--[if mso | IE]>\r\n<table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"vertical-align:top;width:600px;\">\r\n<![endif]-->\r\n                      <div class='dys-column-per-100 outlook-group-fix'\r\n                        style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>\r\n                        <table border='0' cellpadding='0' cellspacing='0' role='presentation' width='100%'>\r\n                          <tbody>\r\n                            <tr>\r\n                              <td style='padding:0px 20px;vertical-align:top;'>\r\n                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style=''\r\n                                  width='100%'>\r\n                                  <tr>\r\n                                    <td align='left' style='font-size:0px;padding:0px;word-break:break-word;'>\r\n                                      <table border='0' cellpadding='0' cellspacing='0'\r\n                                        style='cellpadding:0;cellspacing:0;color:#000000;font-family:Helvetica, Arial, sans-serif;font-size:13px;line-height:22px;table-layout:auto;width:100%;'\r\n                                        width='100%'>\r\n                                        <tr>\r\n                                          <td align='left'>\r\n                                            <a href='#'>\r\n                                              <img align='left' alt='Logo' height='33' padding='5px'\r\n                                                src='https://swu-cs-assets.s3.amazonaws.com/OSET/oxy-logo.png'\r\n                                                width='120' />\r\n                                            </a>\r\n                                          </td>\r\n                                          <td align='right' style='vertical-align:bottom;' width='34px'>\r\n                                            <a href='#'>\r\n                                              <img alt='Twitter' height='22'\r\n                                                src='https://s3.amazonaws.com/swu-cs-assets/OSET/social/Twitter_grey.png'\r\n                                                width='22' />\r\n                                            </a>\r\n                                          </td>\r\n                                          <td align='right' style='vertical-align:bottom;' width='34px'>\r\n                                            <a href='#'>\r\n                                              <img alt='Facebook' height='22'\r\n                                                src='https://swu-cs-assets.s3.amazonaws.com/OSET/social/f_grey.png'\r\n                                                width='22' />\r\n                                            </a>\r\n                                          </td>\r\n                                          <td align='right' style='vertical-align:bottom;' width='34px'>\r\n                                            <a href='#'>\r\n                                              <img alt='Instagram' height='22'\r\n                                                src='https://swu-cs-assets.s3.amazonaws.com/OSET/social/instagrey.png'\r\n                                                width='22' />\r\n                                            </a>\r\n                                          </td>\r\n                                        </tr>\r\n                                      </table>\r\n                                    </td>\r\n                                  </tr>\r\n                                </table>\r\n                              </td>\r\n                            </tr>\r\n                          </tbody>\r\n                        </table>\r\n                      </div>\r\n                      <!--[if mso | IE]>\r\n</td></tr></table>\r\n<![endif]-->\r\n                    </td>\r\n                  </tr>\r\n                </tbody>\r\n              </table>\r\n            </div>\r\n          </div>\r\n          <!--[if mso | IE]>\r\n</v:textbox></v:rect>\r\n<![endif]-->\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table>\r\n  <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation'\r\n    style='background:#f7f7f7;background-color:#f7f7f7;width:100%;'>\r\n    <tbody>\r\n      <tr>\r\n        <td>\r\n          <div style='margin:0px auto;max-width:600px;'>\r\n            <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%;'>\r\n              <tbody>\r\n                <tr>\r\n                  <td style='direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;'>\r\n                    <!--[if mso | IE]>\r\n<table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"vertical-align:top;width:600px;\">\r\n<![endif]-->\r\n                    <div class='dys-column-per-100 outlook-group-fix'\r\n                      style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>\r\n                      <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top;'\r\n                        width='100%'>\r\n                        <tr>\r\n                          <td align='center' style='font-size:0px;padding:10px 25px;word-break:break-word;'>\r\n                            <div\r\n                              style='color:#4d4d4d;font-family:Oxygen, Helvetica neue, sans-serif;font-size:32px;font-weight:700;line-height:37px;text-align:center;'>\r\n                              Chúc mừng bạn đã đặt hàng thành công\r\n                            </div>\r\n                          </td>\r\n                        </tr>\r\n                        <tr>\r\n                          <td align='center' style='font-size:0px;padding:10px 25px;word-break:break-word;'>\r\n                            <div\r\n                              style='color:#777777;font-family:Oxygen, Helvetica neue, sans-serif;font-size:14px;line-height:21px;text-align:center;'>\r\n                              Cảm ơn bạn đã lựa chọn VietInk, đơn hàng của bạn sẽ được giao sau 3-5 ngày kể từ khi bạn\r\n                              nhận mail này, để giải đáp thắc mắc, bạn vui lòng liên hệ qua website của chúng mình\r\n                              nhé!\r\n                            </div>\r\n                          </td>\r\n                        </tr>\r\n                      </table>\r\n                    </div>\r\n                    <!--[if mso | IE]>\r\n</td></tr></table>\r\n<![endif]-->\r\n                  </td>\r\n                </tr>\r\n              </tbody>\r\n            </table>\r\n          </div>\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table>\r\n  <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation'\r\n    style='background:#f7f7f7;background-color:#f7f7f7;width:100%;'>\r\n    <tbody>\r\n      <tr>\r\n        <td>\r\n          <div style='margin:0px auto;max-width:600px;'>\r\n            <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%;'>\r\n              <tbody>\r\n                <tr>\r\n                  <td style='direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;'>\r\n\r\n                    <!--[if mso | IE]>\r\n</td>\r\n<![endif]-->\r\n                    <!-- empty column to create gap -->\r\n                    <!--[if mso | IE]>\r\n<td style=\"vertical-align:top;width:30px;\">\r\n<![endif]-->\r\n                    <div class='dys-column-per-5 outlook-group-fix'\r\n                      style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>\r\n                      <table border='0' cellpadding='0' cellspacing='0' role='presentation' width='100%'>\r\n                        <tbody>\r\n                          <tr>\r\n                            <td style='background-color:#FFFFFF;padding:0;vertical-align:top;'>\r\n                              <table border='0' cellpadding='0' cellspacing='0' role='presentation' style=''\r\n                                width='100%'>\r\n                              </table>\r\n                            </td>\r\n                          </tr>\r\n                        </tbody>\r\n                      </table>\r\n                    </div>\r\n                    <!--[if mso | IE]>\r\n</td><td style=\"vertical-align:top;width:270px;\">\r\n<![endif]-->\r\n                    <div class='dys-column-per-45 outlook-group-fix'\r\n                      style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>\r\n                      <table border='0' cellpadding='0' cellspacing='0' role='presentation' width='100%'>\r\n                        <tbody>\r\n                          <tr>\r\n                            <td\r\n                              style='background-color:#ffffff;border:1px solid #e5e5e5;padding:15px;vertical-align:top;'>\r\n                              <table border='0' cellpadding='0' cellspacing='0' role='presentation' style=''\r\n                                width='100%'>\r\n                                \r\n                                <tr>\r\n                                  <td align='left' style='font-size:0px;padding:0px ;word-break:break-word;'>\r\n                                    <div\r\n                                      style='color:#4d4d4d;font-family:Oxygen, Helvetica neue, sans-serif;font-size:18px;font-weight:700;line-height:25px;text-align:left;'>\r\n                                      Mã đơn hàng\r\n                                    </div>\r\n                                  </td>\r\n                                </tr>\r\n                                <tr>\r\n                                  <td align='left' style='font-size:0px;padding:0px;word-break:break-word;'>\r\n                                    <div\r\n                                      style='color:#777777;font-family:Oxygen, Helvetica neue, sans-serif;font-size:14px;line-height:22px;text-align:left;'>\r\n                                      #{Order.OrderId}\r\n                                    </div>\r\n                                  </td>\r\n                                </tr>\r\n                                <tr>\r\n                                  <td align='left' style='font-size:0px;padding:0px ;word-break:break-word;'>\r\n                                    <div\r\n                                      style='color:#4d4d4d;font-family:Oxygen, Helvetica neue, sans-serif;font-size:18px;font-weight:700;line-height:25px;text-align:left;'>\r\n                                      Ngày đặt hàng\r\n                                    </div>\r\n                                  </td>\r\n                                </tr>\r\n                                <tr>\r\n                                  <td align='left' style='font-size:0px;padding:0px;word-break:break-word;'>\r\n                                    <div\r\n                                      style='color:#777777;font-family:Oxygen, Helvetica neue, sans-serif;font-size:14px;line-height:22px;text-align:left;'>\r\n                                      {Order.OrderDate}\r\n                                    </div>\r\n                                  </td>\r\n                                </tr>\r\n                              </table>\r\n                            </td>\r\n                          </tr>\r\n                        </tbody>\r\n                      </table>\r\n                    </div>\r\n                    <!--[if mso | IE]>\r\n<table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"vertical-align:top;width:270px;\">\r\n<![endif]-->\r\n                    <div class='dys-column-per-45 outlook-group-fix'\r\n                      style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>\r\n                      <table border='0' cellpadding='0' cellspacing='0' role='presentation' width='100%'>\r\n                        <tbody>\r\n                          <tr>\r\n                            <td\r\n                              style='background-color:#ffffff;border:1px solid #e5e5e5;padding:15px;vertical-align:top;'>\r\n                              <table border='0' cellpadding='0' cellspacing='0' role='presentation' style=''\r\n                                width='100%'>\r\n                                <tr>\r\n                                  <td align='left' style='font-size:0px;padding:0px ;word-break:break-word;'>\r\n                                    <div\r\n                                      style='color:#4d4d4d;font-family:Oxygen, Helvetica neue, sans-serif;font-size:18px;font-weight:700;line-height:25px;text-align:left;'>\r\n                                      Địa chỉ nhận hàng\r\n                                    </div>\r\n                                  </td>\r\n                                </tr>\r\n                                <tr>\r\n                                  <td align='left' style='font-size:0px;padding:0px;word-break:break-word;'>\r\n                                    <div\r\n                                      style='color:#777777;font-family:Oxygen, Helvetica neue, sans-serif;font-size:14px;line-height:23px;text-align:left;'>\r\n                                      {Order.ShipAddress}, {Order.ShipRegion}, {Order.ShipCity}\r\n                                    </div>\r\n                                  </td>\r\n                                </tr>\r\n\r\n                                <tr>\r\n                                  <td align='left' style='font-size:0px;padding:0px ;word-break:break-word;'>\r\n                                    <div\r\n                                      style='color:#4d4d4d;font-family:Oxygen, Helvetica neue, sans-serif;font-size:18px;font-weight:700;line-height:25px;text-align:left;'>\r\n                                      Số điện thoại\r\n                                    </div>\r\n                                  </td>\r\n                                </tr>\r\n                                <tr>\r\n                                  <td align='left' style='font-size:0px;padding:0px;word-break:break-word;'>\r\n                                    <div\r\n                                      style='color:#777777;font-family:Oxygen, Helvetica neue, sans-serif;font-size:14px;line-height:23px;text-align:left;'>\r\n                                      {Order.PhoneNumber}\r\n                                    </div>\r\n                                  </td>\r\n                                </tr>\r\n<tr>\r\n                                  <td align='left' style='font-size:0px;padding:0px ;word-break:break-word;'>\r\n                                    <div\r\n                                      style='color:#4d4d4d;font-family:Oxygen, Helvetica neue, sans-serif;font-size:18px;font-weight:700;line-height:25px;text-align:left;'>\r\n                                      Người nhận </div>\r\n                                  </td>\r\n                                </tr>\r\n                                <tr>\r\n                                  <td align='left' style='font-size:0px;padding:0px;word-break:break-word;'>\r\n                                    <div\r\n                                      style='color:#777777;font-family:Oxygen, Helvetica neue, sans-serif;font-size:14px;line-height:23px;text-align:left;'>\r\n                                      {User.LastName} {User.FirstName} </div>\r\n                                  </td>\r\n                                </tr>                              </table>\r\n                            </td>\r\n                          </tr>\r\n                        </tbody>\r\n                      </table>\r\n                    </div>\r\n                    <!--[if mso | IE]>\r\n</td></tr></table>\r\n<![endif]-->\r\n                  </td>\r\n                </tr>\r\n              </tbody>\r\n            </table>\r\n          </div>\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table>\r\n  <!--[if mso | IE]>\r\n<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:600px;\" width=\"600\"><tr><td style=\"line-height:0px;font-size:0px;mso-line-height-rule:exactly;\">\r\n<![endif]-->\r\n  <div style='background:#FFFFFF;background-color:#FFFFFF;margin:0px auto;max-width:600px;'>\r\n    <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation'\r\n      style='background:#FFFFFF;background-color:#FFFFFF;width:100%;'>\r\n      <tbody>\r\n        <tr>\r\n          <td style='direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;'>\r\n            <!--[if mso | IE]>\r\n<table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"vertical-align:top;width:600px;\">\r\n<![endif]-->\r\n            <div class='dys-column-per-100 outlook-group-fix'\r\n              style='direction:ltr;display:inline-block;font-size:13px;text-align:left;vertical-align:top;width:100%;'>\r\n              <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top;'\r\n                width='100%'>\r\n                <tr>\r\n                  <td align='left' style='font-size:0px;padding:10px 25px;word-break:break-word;'>\r\n                    <table border='0' cellpadding='0' cellspacing='0'\r\n                      style=\"cellpadding:0;cellspacing:0;color:#777777;font-family:'Oxygen', 'Helvetica Neue', helvetica, sans-serif;font-size:14px;line-height:21px;table-layout:auto;width:100%;\"\r\n                      width='100%'>\r\n                      <tr>\r\n                        <th\r\n                          style='text-align: left; border-bottom: 1px solid #cccccc; color: #4d4d4d; font-weight: 700; padding-bottom: 5px;'\r\n                          width='50%'>\r\n                          Item\r\n                        </th>\r\n                        <th\r\n                          style='text-align: right; border-bottom: 1px solid #cccccc; color: #4d4d4d; font-weight: 700; padding-bottom: 5px;'\r\n                          width='15%'>\r\n                          Qty\r\n                        </th>\r\n                        <th\r\n                          style='text-align: right; border-bottom: 1px solid #cccccc; color: #4d4d4d; font-weight: 700; padding-bottom: 5px; '\r\n                          width='15%'>\r\n                          Total\r\n                        </th>\r\n                      </tr>\r\n                    </table>\r\n                  </td>\r\n                </tr>\r\n                <tr>\r\n                  <td align='left' style='font-size:0px;padding:10px 25px;word-break:break-word;'>\r\n                    <table border='0' cellpadding='0' cellspacing='0'\r\n                      style='cellpadding:0;cellspacing:0;color:#000000;font-family:Helvetica, Arial, sans-serif;font-size:13px;line-height:22px;table-layout:auto;width:100%;'\r\n                      width='100%'>\r\n                      <!-- product -->";
            foreach (var item in OrderDetails) {
                var product = _context.Products.SingleOrDefault(p => p.ProductId == item.ProductId);

                string baseAddress = "https://vietink.shop/";
                string path = $"/ProductImages/{product.Image}";
                string encodedPath = HttpUtility.UrlPathEncode(path);
                string absoluteUrl = baseAddress + encodedPath;

                html += $"<tr style=\"font-size:14px; line-height:19px; font-family: 'Oxygen', 'Helvetica Neue', helvetica, sans-serif; color:#777777\">\r\n                        <td width='50%'>\r\n                          <table cellpadding='0' cellspacing='0' style=\"font-size:14px; line-height:19px; font-family: 'Oxygen', 'Helvetica Neue', helvetica, sans-serif;\" width='100%'>\r\n                            <tbody>\r\n                              <tr>\r\n                                <td style='padding-right:5px;' width='35%'>\r\n                                  <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px;'>\r\n                                    <tbody>\r\n                                      <tr>\r\n                                        <td style='width:110px;'>\r\n                                          <a href='#' target='_blank'>\r\n                                            <img alt='item1' height='auto'\r\n                                              src='{absoluteUrl}'\r\n                                              style='border: 1px solid #e6e6e6;border-radius:4px;display:block;font-size:13px;height:auto;outline:none;text-decoration:none;width:100%;'\r\n                                              width='110' />\r\n                                          </a>\r\n                                        </td>\r\n                                      </tr>\r\n                                    </tbody>\r\n                                  </table>\r\n                                </td>\r\n                                <td style=\"text-align:left; font-size:14px; line-height:19px; font-family: ' oxygen', 'helvetica neue', helvetica, sans-serif; color: #777777;\">\r\n                                  <span style='color: #4d4d4d; font-weight:bold;'> {product.ProductName} </span>\r\n                                  <br />\r\n                                  {product.UnitPrice} đ\r\n                                </td>\r\n                              </tr>\r\n                            </tbody>\r\n                          </table>\r\n                        </td>\r\n                        <td style='text-align:center; ' width='10%'>\r\n                          {item.Quantity}\r\n                        </td>\r\n                        <td style='text-align:right; ' width='10%'>\r\n                          {item.Quantity * item.UnitPrice} đ\r\n                        </td>\r\n                      </tr>";
            }
            html += $"</table>\r\n                  </td>\r\n                </tr>\r\n                <tr>\r\n                  <td align='left' style='font-size:0px;padding:10px 25px;word-break:break-word;'>\r\n                    <table border='0' cellpadding='0' cellspacing='0'\r\n                      style='cellpadding:0;cellspacing:0;color:#000000;font-family:Helvetica, Arial, sans-serif;font-size:13px;line-height:22px;table-layout:auto;width:100%;'\r\n                      width='100%'>\r\n                      <tr\r\n                        style=\"font-size:14px; line-height:19px; font-family: 'Oxygen', 'Helvetica Neue', helvetica, sans-serif; color:#777777\">\r\n                        <td width='50%'>\r\n                        </td>\r\n                        <td style='text-align:right; padding-right: 10px; border-top: 1px solid #cccccc;'>\r\n                          <span style='padding:8px 0px; display: inline-block;'>\r\n                            Tổng\r\n                          </span>\r\n                          <br />\r\n                          <span style='padding-bottom:8px; display: inline-block;'>\r\n                            VAT\r\n                          </span>\r\n                          <br />\r\n                          <span style='padding-bottom:8px; display: inline-block;'>\r\n                            Phí vận chuyển\r\n                          </span>\r\n                          <br />\r\n                          <span style='display: inline-block;font-weight: bold; color: #4d4d4d'>\r\n                            Thành tiền\r\n                          </span>\r\n                        </td>\r\n                        <td style='text-align: right; border-top: 1px solid #cccccc;'>\r\n                          <span style='padding:8px 0px; display: inline-block;'>\r\n                            {Order.Freight - 22000}\r\n                          </span>\r\n                          <br />\r\n                          <span style='padding-bottom:8px; display: inline-block;'>\r\n                            0.0 đ\r\n                          </span>\r\n                          <br />\r\n                          <span style='padding-bottom:8px; display: inline-block;'>\r\n                            22.000 đ\r\n                          </span>\r\n                          <br />\r\n                          <span style='display: inline-block;font-weight: bold; color: #4d4d4d'>\r\n                            {Order.Freight}\r\n                          </span>\r\n                        </td>\r\n                      </tr>\r\n                    </table>\r\n                  </td>\r\n                </tr>\r\n              </table>\r\n            </div>\r\n            <!--[if mso | IE]>\r\n</td></tr></table>\r\n<![endif]-->\r\n          </td>\r\n        </tr>\r\n      </tbody>\r\n    </table>\r\n  </div>\r\n  <!--[if mso | IE]>\r\n</td></tr></table>\r\n<![endif]-->\r\n  <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation'\r\n    style='background:#f7f7f7;background-color:#f7f7f7;width:100%;'>\r\n    <tbody>\r\n      <tr>\r\n        <td>\r\n          <div style='margin:0px auto;max-width:600px;'>\r\n            <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%;'>\r\n              <tbody>\r\n                <tr>\r\n                  <td style='direction:ltr;font-size:0px;padding:20px;text-align:center;vertical-align:top;'>\r\n                    <!--[if mso | IE]>\r\n<table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"vertical-align:top;width:540px;\">\r\n<![endif]-->\r\n\r\n                    <!--[if mso | IE]>\r\n</td></tr></table>\r\n<![endif]-->\r\n                  </td>\r\n                </tr>\r\n              </tbody>\r\n            </table>\r\n          </div>\r\n        </td>\r\n      </tr>\r\n    </tbody>\r\n  </table>\r\n</div>";
            return html;
        }
    
    }
}
