using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using ExpenseTracker.Models;
using ExpenseTracker.Models.Pagination;
using ExpenseTracker.Server.Entities;
using ExpenseTracker.Server.Helpers;
using ExpenseTracker.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Server.Controllers
{
    [Route("api/[controller]")]
    public class InvoiceController : Controller
    {
        private readonly IExpenseTrackerService _expenseTrackerService;
        private IConverter _converter;

        public InvoiceController(IExpenseTrackerService expenseTrackerService, IConverter converter)
        {
            _expenseTrackerService = expenseTrackerService;
            _converter = converter;
        }

        //POST: api/invoice
        [HttpPost]
        public async Task<ActionResult<InvoiceVm>> Create([FromBody] InvoiceDto dto)
        {

            var invoice = await _expenseTrackerService.AddInvoice(dto);

            return CreatedAtAction(nameof(Create), new { id = invoice.Id }, invoice);

        }

        //GET: api/invoice
        [HttpGet]
        public async Task<ActionResult<PaginatedData<List<InvoiceVm>>>> GetAll([FromQuery] PaginatedQuery paginatedQuery)
        {

            var invoice = await _expenseTrackerService.GetAllInvoices(paginatedQuery);

            return Ok(invoice);
        }

        //GET: api/invoice/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceVm>> GetById(int id)
        {

            var invoice = await _expenseTrackerService.GetInvoice(id);

            return Ok(invoice);
        }

        //POST: api/invoice
        [HttpPut("{id}")]
        public async Task<ActionResult<InvoiceVm>> Update(int id, [FromBody] InvoiceUpdateDto dto)
        {
            dto.Id = id;
            var invoice = await _expenseTrackerService.UpdateInvoice(dto);

            return Ok(invoice);
        }

        //Delete: api/invoice/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<InvoiceVm>> Delete(int id)
        {

            var invoice = await _expenseTrackerService.DeleteInvoice(id);

            return NoContent();
        }

        [HttpGet("print-all")]
        public async Task<IActionResult> CreatePDFAsync([FromQuery] PaginatedQuery paginatedQuery)
        {

            var invoice = await _expenseTrackerService.GetAllInvoices(paginatedQuery);
            DateTime time = DateTime.Now;

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLString(invoice, paginatedQuery),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = $"Report generated at {time}" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = _converter.Convert(pdf);

            return File(file, "application/pdf", $"Invoice_Report.pdf");
        }


        [HttpGet("print-{id}")]
        public async Task<IActionResult> CreatePDFByIdAsync(int id)
        {

            var invoice = await _expenseTrackerService.GetInvoice(id);
            DateTime time = DateTime.Now;

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLStringForInvoice(invoice),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = $"Report generated at {time}" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = _converter.Convert(pdf);

            return File(file, "application/pdf", $"Invoice_Report_{invoice.InvoiceNo}.pdf");
        }
    }

}
