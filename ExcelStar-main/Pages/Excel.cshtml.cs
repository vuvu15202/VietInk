using ClosedXML.Excel;
using ExcelStar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExcelStar.Pages;

public class ExcelModel : PageModel {
    private readonly ILogger<IndexModel> _logger;

    public ExcelModel(ILogger<IndexModel> logger) {
        _logger = logger;
    }

    public FileResult OnGet() {
        var data = Student.GetStudents();

        using (var workbook = new XLWorkbook()) {
            IXLWorksheet worksheet =
            workbook.Worksheets.Add("Students");
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "First";
            worksheet.Cell(1, 3).Value = "Last";
            worksheet.Cell(1, 4).Value = "School";

            IXLRange range = worksheet.Range(worksheet.Cell(1, 1).Address, worksheet.Cell(1, 4).Address);
            range.Style.Fill.SetBackgroundColor(XLColor.Almond);

            int index = 1;

            foreach (var item in data) {
                index++;

                worksheet.Cell(index, 1).Value = item.Id;
                worksheet.Cell(index, 2).Value = item.FirstName;
                worksheet.Cell(index, 3).Value = item.LastName;
                worksheet.Cell(index, 4).Value = item.School;

            }

            using (var stream = new MemoryStream()) {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                var strDate = DateTime.Now.ToString("yyyyMMdd");
                string filename = string.Format($"Students_{strDate}.xlsx");

                return File(content, contentType, filename);
            }
        }
    }
}