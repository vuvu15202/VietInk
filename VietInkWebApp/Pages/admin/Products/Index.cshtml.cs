using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using VietInkWebApp.Entities;

namespace VietInkWebApp.Pages.admin.Products
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly VietInkWebApp.Entities.TattooshopContext _context;

        public IndexModel(VietInkWebApp.Entities.TattooshopContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Product = await _context.Products.ToListAsync();
            }
        }


        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "xlxs")]
        [Display(Name = "Choose file(s) to upload")]
        [BindProperty]
        public IFormFile FileUploads { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid || _context.Products == null || Product == null)
            //  {
            //      return Page();
            //  

            var listProduct = new List<Product>();

            if (FileUploads != null && FileUploads.Length != 0)
            {
                using (var stream = new MemoryStream())
                {
                    await FileUploads.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowcount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowcount; row++)
                        {
                            listProduct.Add(new Product
                            {
                                ProductName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                CategoryName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                QuantityPerUnit = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                UnitPrice = Int32.Parse(worksheet.Cells[row, 4].Value.ToString().Trim()),
                                UnitsInStock = Int32.Parse(worksheet.Cells[row, 5].Value.ToString().Trim()),
                                Image = worksheet.Cells[row, 6].Value.ToString().Trim(),
                                Discontinued = (worksheet.Cells[row, 7].Value.ToString().Trim().Equals("True")) == true ? true : false,
                            });

                        }
                    }
                }
            }

            if (listProduct.Count >= 0) _context.Products.AddRange(listProduct);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
