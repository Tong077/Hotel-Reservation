using H_application.DTOs.PaymentMethodDto;
using H_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Service
{
    public interface IPaymentMethodService
    {
        Task<bool> CreatePaymentMethodAsync(PaymentMethodDtoCreate payment, CancellationToken cancellationToken = default);
        Task<bool> UpdatePaymentMethodAsync(PaymentMethodDtoUpdate payment, CancellationToken cancellationToken = default);
        Task<bool> DeletePaymentMethodAsync(PaymentMethodDtoUpdate payment, CancellationToken cancellationToken = default);
        Task<PaymentMethodDtoUpdate> GetPaymentMethodById (int Id , CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentMethodResponse>> GetAllPaymentMethods(CancellationToken cancellationToken = default);
    }
}
