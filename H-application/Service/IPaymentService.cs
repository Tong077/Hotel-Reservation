using H_application.DTOs.Payment;
using H_Domain.Models;

namespace H_application.Service
{
    public interface IPaymentService
    {
        Task<bool> CreatePaymentAsync(PaymentDtoCreate payment,CancellationToken cancellationToken);
        Task<bool> CreatePaymentForReservationAsync(PaymentDtoCreate dto, CancellationToken cancellationToken);
        Task<bool> UpdatePaymentAsync(PaymentDtoUpdate payment,CancellationToken cancellationToken);
        Task<bool> DeletePaymentAsync(PaymentDtoUpdate payment, CancellationToken cancellation);
        Task<IEnumerable<PaymentResponse>> GetAllPaymentResponsesAsync(CancellationToken cancellationToken);
        Task<PaymentDtoUpdate> GetPaymentByIdAsync(int id, CancellationToken cancellationToken);
        Task<PaymentResponse> GetAllTotalPayment(string currency, CancellationToken cancellationToken);
        Task<int> GetPaymentsCountByStatusAsync(string status, CancellationToken cancellationToken);
        
        Task<bool> UpdatePaymentWithInvoiceAsync(PaymentDtoUpdate dto, CancellationToken cancellation = default);
    }
}
