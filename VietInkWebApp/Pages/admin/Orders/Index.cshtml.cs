using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VietInkWebApp.Entities;

namespace VietInkWebApp.Pages.admin.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly VietInkWebApp.Entities.TattooshopContext _context;

        public IndexModel(VietInkWebApp.Entities.TattooshopContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;
        public int Status { get; set; } = 0;
        public int CurrentFilter { get; set; }


        public async Task OnGetAsync(int? processing, int? shipped, int? cancel, int? filter)
        {
            if (_context.Orders != null)
            {
                try
                {
                    if (processing != null)
                    {
                        var order = _context.Orders.SingleOrDefault(o => o.OrderId == (int)processing);
                        if (order != null)
                        {
                            order.RequiredDate = DateTime.Now.AddDays(5);
                            _context.Orders.Update(order);
                            await _context.SaveChangesAsync();
                        }
                        Status = 1;
                    }

                    if (shipped != null)
                    {
                        var order = _context.Orders.SingleOrDefault(o => o.OrderId == (int)shipped);
                        if (order != null)
                        {
                            order.ShippedDate = DateTime.Now;
                            await _context.SaveChangesAsync();
                        }
                        Status = 1;
                    }

                    if (cancel != null)
                    {
                        var order = _context.Orders.SingleOrDefault(o => o.OrderId == (int)cancel);
                        if (order != null)
                        {
                            order.RequiredDate = order.OrderDate;
                            order.ShippedDate = order.OrderDate;
                            await _context.SaveChangesAsync();
                        }
                        Status = 1;
                    }
                }
                catch(Exception ex)
                {
                    Status = 2;
                }



                if (filter != null)
                {
                    switch(filter)
                    {
                        case 1: Order = await _context.Orders.Include(o => o.User).Where(o => o.User != null 
                            && o.RequiredDate == null 
                            && o.ShippedDate == null).ToListAsync();  
                            break;
                        case 2: Order = await _context.Orders.Include(o => o.User).Where(o => o.User != null 
                            && o.RequiredDate != null 
                            && o.ShippedDate == null).ToListAsync(); 
                            break;
                        case 3: Order = await _context.Orders.Include(o => o.User).Where(o => o.User != null 
                            && o.RequiredDate != null 
                            && o.ShippedDate != null 
                            && o.RequiredDate != o.ShippedDate ).ToListAsync(); 
                            break;
                        case 4:
                            Order = await _context.Orders.Include(o => o.User).Where(o => o.User != null
                            && o.RequiredDate != null
                            && o.ShippedDate != null
                            && o.RequiredDate == o.ShippedDate).ToListAsync();
                            break;
                        case 5:
                            Order = await _context.Orders.Include(o => o.User).Where(o => o.User != null).ToListAsync();
                            break;
                    }
                    CurrentFilter = (int)filter;
                }
                else
                {
                    Order = await _context.Orders.Include(o => o.User).Where(o => o.User != null
                            && o.RequiredDate == null
                            && o.ShippedDate == null).OrderBy(o => o.OrderDate).ToListAsync();
                    CurrentFilter = 1;
                }
            }
        }

        public async Task OnPostAsync()
        {
            if (_context.Orders != null)
            {
                
            }
        }
    }
}
