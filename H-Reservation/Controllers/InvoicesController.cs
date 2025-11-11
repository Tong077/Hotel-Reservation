using AspNetCoreGeneratedDocument;
using H_application.DTOs.InvoicesDto;
using H_application.Service;
using H_Domain.DataContext;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace H_Reservation.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IInvoicesServicecs _invoice;

        public InvoicesController(IInvoicesServicecs invoice)
        {
            _invoice = invoice;
        }

        public async Task<IActionResult> Index()
        {
            var invoices = await _invoice.GetAllInvoicesAsync(default);
            return View("Index", invoices);
        }
        public async Task<IActionResult> Create(int reservationId, CancellationToken cancellation)
        {
            var invoice = await _invoice.CreateInvoiceAsync(reservationId, cancellation);
            return RedirectToAction(nameof(Details), new { id = invoice.InvoiceId });
        }

       
        public async Task<IActionResult> Details(int id, CancellationToken cancellation)
        {
            var invoice = await _invoice.GetInvoiceByIdAsync(id, cancellation);
            if (invoice == null)
                return NotFound();

            return View(invoice);
        }

        
        public async Task<IActionResult> Print(int id, CancellationToken cancellation)
        {
            var invoice = await _invoice.GetInvoiceByIdAsync(id, cancellation);
            if (invoice == null)
                return NotFound();

            return View(invoice);
        }
        [HttpGet]
        public IActionResult PrintMultiple()
        {
            return View("PrintMultiple");
        }

        [HttpPost]
        public async Task<IActionResult> PrintMultiple([FromForm] List<int> invoiceIds, CancellationToken cancellation, bool separatePages = true)
        {
            if (invoiceIds == null || !invoiceIds.Any())
                return BadRequest("No invoices selected.");

            var invoices = new List<InvoicesResponse>();

            foreach (var id in invoiceIds)
            {
                var invoice = await _invoice.GetInvoiceByIdAsync(id, cancellation);
                if (invoice != null) invoices.Add(invoice);
            }

            if (!invoices.Any())
                return NotFound("No valid invoices found.");

            ViewData["SeparatePages"] = separatePages; // control page break
            return View("PrintMultiple", invoices);
        }


    }

}

