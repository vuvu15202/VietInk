using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VietInkWebApp.Pagging;
using VietInkWebApp.Entities;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.Data.SqlClient;

namespace VietInkWebApp.Pages.products
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

        //public IList<Product> Products { get; set; } = default!;


        public string CurrentSearchString { get; set; }
        public string CurrentCategoryName { get; set; }
        public string CurrentIsInStock { get; set; }
        public string CurrentSortPrice { get; set; }

        public IList<Product> Product { get; set; } = default!;
        public PaginatedList<Product> Products { get; set; }

        public async Task OnGetAsync(string searchString, string categoryName, string isInStock,string sortPrice,  int? pageIndex)
        {
            //CurrentSort = sortOrder;
            //NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //QuantitySort = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";


            //CurrentSearchString = String.IsNullOrEmpty(searchString) ? "" : searchString;
            //CurrentSortPrice = String.IsNullOrEmpty(sortPrice) ? "desc" : sortPrice;

            //if (_context.Books != null)
            //{
            //    Book = await _context.Books.ToListAsync();
            //}

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = CurrentSearchString;
            }
            CurrentSearchString = searchString;

            IQueryable<Product> productIQ = from b in _context.Products select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                productIQ = productIQ.Where(s => s.ProductName.Contains(searchString));

            }

            if (!String.IsNullOrEmpty(categoryName))
            {
                if(!categoryName.Contains("All")) productIQ = productIQ.Where(s => s.CategoryName.Contains(categoryName));

            }
            CurrentCategoryName = String.IsNullOrEmpty(categoryName) ? "" : categoryName;


            if (!String.IsNullOrEmpty(isInStock))
            {
                if (isInStock.Contains("instock"))
                {
                    productIQ = productIQ.Where(s => s.UnitsInStock > 0);
                }
                else if (isInStock.Contains("outofstock"))
                {
                    productIQ = productIQ.Where(s => s.UnitsInStock <= 0);

                }

            }
            CurrentIsInStock = String.IsNullOrEmpty(isInStock) ? "" : isInStock;


            switch (sortPrice)
            {
                case "desc":
                    productIQ = productIQ.OrderByDescending(s => s.UnitPrice);
                    break;
                case "asc":
                    productIQ = productIQ.OrderBy(s => s.UnitPrice);
                    break;
                default:
                    productIQ = productIQ.OrderBy(s => s.ProductName);
                    break;
            }
            CurrentSortPrice = String.IsNullOrEmpty(sortPrice) ? "desc" : sortPrice;

            var pageSize = Configuration.GetValue("PageSize", 9);
            Products = await PaginatedList<Product>.CreateAsync(productIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

        }
        //public async Task<IActionResult> OnPostAsync(string searchString, int? pageIndex)
        //{
        //    //if (!ModelState.IsValid || _context.Products == null || Product == null)
        //    //  {
        //    //      return Page();
        //    //  
        //    IQueryable<Product> productIQ = from b in _context.Products select b;
        //    if (searchString != null)
        //    {
        //        pageIndex = 1;
        //        productIQ = productIQ.Where(p => p.ProductName.Contains(searchString));

        //    }
        //    CurrentSearchString = searchString;

        //    var pageSize = Configuration.GetValue("PageSize", 9);
        //    Products = await PaginatedList<Product>.CreateAsync(productIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

        //    return RedirectToPage("./Index");
        //}
    }
}
