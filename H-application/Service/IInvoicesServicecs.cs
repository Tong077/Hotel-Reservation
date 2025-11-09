using H_application.DTOs.InvoicesDto;
using H_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Service
{
    public interface IInvoicesServicecs
    {
        Task<List<InvoicesResponse>> GetAllInvoicesAsync(CancellationToken cancellationToken = default);
        Task<InvoicesResponse?> GetInvoiceByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> CreateInvoiceAsync(InvoicesDtoCreate invoice, CancellationToken cancellationToken = default);
        Task<InvoicesDtoCreate> CreateInvoiceAsync(int reservationId, CancellationToken cancellation = default);
    }
}
