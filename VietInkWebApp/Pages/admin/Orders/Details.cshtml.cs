using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VietInkWebApp.Entities;

namespace VietInkWebApp.Pages.admin.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly VietInkWebApp.Entities.TattooshopContext _context;

        public DetailsModel(VietInkWebApp.Entities.TattooshopContext context)
        {
            _context = context;
        }

      public Order Order { get; set; } = default!;
        public IList<OrderDetail> OrderDetail { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            else 
            {
                Order = order;
                if (_context.OrderDetails != null)
                {
                    OrderDetail = await _context.OrderDetails
                    .Include(o => o.Order)
                    .Include(o => o.Product).Where(od => od.OrderId == id).ToListAsync();
                }
            }
            return Page();
        }
    }
}
