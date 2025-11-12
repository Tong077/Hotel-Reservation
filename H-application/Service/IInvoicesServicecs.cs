using H_application.DTOs.InvoicesDto;

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
