using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ASPNetCore.PdfCreate_UsingiTextSharp.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data;
using FastMember;

namespace ASPNetCore.PdfCreate_UsingiTextSharp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult PdfCreate()
        {
            DataTable dataTable = new DataTable();
            dataTable.Load(ObjectReader.Create(new List<Customer>
            {
                new Customer{Id=1,Name="Kübay"},
                new Customer{Id=2,Name="Mehmet"}
            }));

            string fileName = Guid.NewGuid() + ".pdf";
            string path =Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/documents/"+fileName);
            var stream = new FileStream(path, FileMode.Create);

            Document document = new Document(PageSize.A4,25f,25f,25f,25f);
            PdfWriter.GetInstance(document, stream);
            document.Open();
            Paragraph paragraph = new Paragraph("This page has been created for Asp.NetCore Pdf File");
            Paragraph paragraph2 = new Paragraph("Table Create");
            PdfPTable pdfPTable = new PdfPTable(dataTable.Columns.Count);
            //pdfPTable.AddCell("Name");
            //pdfPTable.AddCell("Surname");
            //pdfPTable.AddCell("Kübay");
            //pdfPTable.AddCell("Telceken");
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                pdfPTable.AddCell(dataTable.Columns[i].ColumnName);
            }
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    pdfPTable.AddCell(dataTable.Rows[i][j].ToString());
                }
            }
            document.Add(paragraph);
            document.Add(paragraph2);
            document.Add(pdfPTable);
            document.Close();
            return File("/documents/"+fileName,"application/pdf",fileName);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
