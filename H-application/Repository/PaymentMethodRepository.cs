using H_application.DTOs.PaymentMethodDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Repository
{
    public class PaymentMethodRepository : IPaymentMethodService
    {
        private readonly EntityContext entityContext;
        public PaymentMethodRepository(EntityContext entityContext)
        {
            this.entityContext = entityContext;
        }

        public async Task<bool> CreatePaymentMethodAsync(PaymentMethodDtoCreate payment, CancellationToken cancellationToken = default)
        {
            var entity = payment.Adapt<PaymentMethod>();
            await entityContext!.PaymentMethods.AddAsync(entity, cancellationToken);
            return await entityContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePaymentMethodAsync(PaymentMethodDtoUpdate payment, CancellationToken cancellationToken = default)
        {
            var entity = payment.Adapt<PaymentMethod>();
            entityContext.PaymentMethods.Remove(entity);
            return await entityContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<PaymentMethodResponse>> GetAllPaymentMethods(CancellationToken cancellationToken = default)
        {
            var paymentMethod = await entityContext.PaymentMethods
                .AsNoTracking().ToListAsync();
            var entity = paymentMethod.Adapt<IEnumerable<PaymentMethodResponse>>();
            return entity;
        }

        public async Task<PaymentMethodDtoUpdate> GetPaymentMethodById(int Id, CancellationToken cancellationToken = default)
        {
            var payment = await entityContext.PaymentMethods
                .FirstOrDefaultAsync(p => p.PaymentMethodId == Id, cancellationToken);
            var entity = payment.Adapt<PaymentMethodDtoUpdate>();
            return entity!;
        }

        public async Task<bool> UpdatePaymentMethodAsync(PaymentMethodDtoUpdate payment, CancellationToken cancellationToken = default)
        {
            var entity = payment.Adapt<PaymentMethod>();
            entityContext.PaymentMethods.Update(entity);
            return await entityContext.SaveChangesAsync() > 0;
        }
    }
}
