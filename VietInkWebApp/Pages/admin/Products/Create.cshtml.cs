using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using VietInkWebApp.Entities;

namespace VietInkWebApp.Pages.admin.Products
{
    public class CreateModel : PageModel
    {
        private readonly VietInkWebApp.Entities.TattooshopContext _context;

        public CreateModel(VietInkWebApp.Entities.TattooshopContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;




        //upload file
        //[Required(ErrorMessage = "Please choose at least one file.")]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        [Display(Name = "Choose file(s) to upload")]
        [BindProperty]
        public IFormFile[] FileUploads { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid || _context.Products == null || Product == null)
            //  {
            //      return Page();
            //  

            if (FileUploads != null)
            {
                foreach (var FileUpload in FileUploads)
                {
                    //var file = Path.Combine(_environment.ContentRootPath, "BookImages", FileUpload.FileName);
                    var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//ProductImages", FileUpload.FileName);
                    Product.Image = FileUpload.FileName;

                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await FileUpload.CopyToAsync(fileStream);
                    }
                }
            }

            _context.Products.Add(Product); 

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
