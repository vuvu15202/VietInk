using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VietInkWebApp.Entities;

namespace VietInkWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly TattooshopContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(ILogger<IndexModel> logger, TattooshopContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            Configuration = configuration;
        }



        public IList<Product> BestSellers { get; set; } = default!;
        public IList<Product> Comboes { get; set; } = default!;
        public IList<Product> Collections { get; set; } = default!;

        public async Task OnGetAsync()
        {
            BestSellers = _context.Products.Where(p => p.Discontinued == false).OrderByDescending(p => p.UnitsInStock).Take(4).ToList();
            Comboes = _context.Products.Where(p => p.Discontinued == false && p.CategoryName.Contains("Combo")).OrderByDescending(p => p.UnitsInStock).Take(10).ToList();
            Collections = _context.Products.Where(p => p.Discontinued == false && p.CategoryName.Contains("Collection")).OrderBy(p => p.UnitsInStock).Take(10).ToList();
        }
    }
}