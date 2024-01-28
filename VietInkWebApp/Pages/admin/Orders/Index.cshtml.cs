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
    public class IndexModel : PageModel
    {
        private readonly VietInkWebApp.Entities.TattooshopContext _context;

        public IndexModel(VietInkWebApp.Entities.TattooshopContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                Order = await _context.Orders
                .Include(o => o.User).ToListAsync();
            }
        }
    }
}
