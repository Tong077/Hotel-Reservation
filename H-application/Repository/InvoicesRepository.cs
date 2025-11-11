using Azure.Core;
using H_application.DTOs.InvoicesDto;
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
    public class InvoicesRepository : IInvoicesServicecs
    {
        private readonly EntityContext _context;
        private readonly ISystemSettingsService _system;

        public InvoicesRepository(EntityContext context, ISystemSettingsService system)
        {
            _context = context;
            _system = system;
        }

        public async Task<bool> CreateInvoiceAsync(InvoicesDtoCreate invoice, CancellationToken cancellationToken = default)
        {
            var entity = invoice.Adapt<Invoice>();
            await _context.Invoices.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }



        //public async Task<InvoicesDtoCreate> CreateInvoiceAsync(int reservationId, CancellationToken cancellation = default)
        //{
        //    var reservation = await _context.Reservations
        //        .FirstOrDefaultAsync(i => i.ReservationId == reservationId, cancellation);
        //    if (reservation == null)
        //        throw new Exception("Reservation not found!");

        //    var payment = await _context.Payments
        //        .FirstOrDefaultAsync(p => p.PaymentId == reservation.PaymentId, cancellation);
        //    if (payment == null)
        //        throw new Exception("No Payment Found for this Reservation.");

        //    var taxValue = await _system.GetValueAsync("TaxRate", cancellation);
        //    var taxRate = decimal.TryParse(taxValue, out var parsedTax) ? parsedTax : 0.10m;

        //    var tax = reservation.TotalPrice * taxRate;
        //    var grandTotal = reservation.TotalPrice + tax;

        //    var invoice = new InvoicesDtoCreate
        //    {
        //        ReservationId = reservationId,
        //        PaymentId = payment.PaymentId,
        //        TotalAmount = reservation.TotalPrice,
        //        TaxAmount = tax,
        //        GrandTotal = grandTotal,
        //        IssuedDate = DateTime.UtcNow,
        //    };

        //    await CreateInvoiceAsync(invoice, cancellation);
        //    return invoice;
        //}

        public async Task<InvoicesDtoCreate> CreateInvoiceAsync(int reservationId, CancellationToken cancellation = default)
        {
            // 1️⃣ Get reservation
            var reservation = await _context.Reservations
                .Include(r => r.ReservationServices) // include services
                    .ThenInclude(rs => rs.Service)
                .Include(r => r.rooms)
                    .ThenInclude(rm => rm.roomType)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId, cancellation);

            if (reservation == null)
                throw new Exception("Reservation not found!");

            // 2️⃣ Get payment
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == reservation.PaymentId, cancellation);
            if (payment == null)
                throw new Exception("No Payment Found for this Reservation.");

            // 3️⃣ Calculate totals
            decimal roomPrice = reservation?.TotalPrice ?? 0; // your room price
            decimal servicesTotal = reservation!.ReservationServices?.Sum(rs => rs.TotalPrice) ?? 0;

            decimal subtotal = roomPrice + servicesTotal;

            var taxValue = await _system.GetValueAsync("TaxRate", cancellation);
            var taxRate = decimal.TryParse(taxValue, out var parsedTax) ? parsedTax : 0.10m;

            decimal tax = subtotal * taxRate;
            decimal grandTotal = subtotal + tax;

            // 4️⃣ Create invoice DTO
            var invoice = new InvoicesDtoCreate
            {
                ReservationId = reservationId,
                PaymentId = payment.PaymentId,
                TotalAmount = subtotal,
                TaxAmount = tax,
                GrandTotal = grandTotal,
                IssuedDate = DateTime.UtcNow,
            };

            // 5️⃣ Save invoice
            await CreateInvoiceAsync(invoice, cancellation);

            return invoice;
        }


        public async Task<List<InvoicesResponse>> GetAllInvoicesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Invoices
                .Include(i => i.Reservation)
                .ThenInclude(r => r!.ReservationServices)
                .Include(i => i.Reservation!.guest)
                .Include(i => i.Reservation!.rooms!.hotel)
                .Include(r => r.Payment)
                .OrderByDescending(r => r.IssuedDate)
                .Select(i => new InvoicesResponse
                {
                    InvoiceId = i.InvoiceId,
                    GuestName = i.Reservation!.guest!.FirstName + " " + i.Reservation.guest.LastName,
                    TotalAmount = i.TotalAmount,
                    TaxAmount = i.TaxAmount,
                    GrandTotal = i.GrandTotal,
                    IssuedDate = i.IssuedDate,
                    HotelName = i.Reservation.rooms!.hotel!.Name,
                    StreetName = i.Reservation.rooms.hotel.City,
                    Address = i.Reservation.rooms.hotel.Address,
                    PaymentId = i.PaymentId,
                    PaymentMethod = i.Payment!.PaymentMethod!.Name,

                    Services = i.Reservation.ReservationServices!
                    .Select(rs => new InvoiceServiceItem
                    {
                        ServiceName = rs.Service!.ServiceName,
                        Quantity = rs.Quantity.HasValue ? (int?)Convert.ToInt32(rs.Quantity.Value) : null,
                        Price = rs.TotalPrice
                    }).ToList()
                }).ToListAsync(cancellationToken);
        }

        public async Task<InvoicesResponse?> GetInvoiceByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Invoices
                .Include(i => i.Reservation)
                    .ThenInclude(r => r!.ReservationServices)
                    .ThenInclude(rs => rs.Service)
                .Include(i => i.Reservation.rooms)
                    .ThenInclude(rm => rm!.hotel) 
                .Include(i => i.Reservation.rooms.roomType)
                .Include(i => i.Payment)
                .Where(i => i.InvoiceId == id)
                .Select(i => new InvoicesResponse
                {
                    InvoiceId = i.InvoiceId,
                    ReservationId = i.ReservationId,
                    GuestName = i.Reservation!.guest!.FirstName + " " + i.Reservation.guest.LastName,
                    RoomNumber = i.Reservation.rooms!.RoomNumber,
                    RoomType = i.Reservation.rooms.roomType!.Name,
                    CheckInDate = i.Reservation.CheckInDate,
                    CheckOutDate = i.Reservation.CheckOutDate,
                    TotalAmount = i.TotalAmount,
                    TaxAmount = i.TaxAmount,
                    GrandTotal = i.GrandTotal,
                    IssuedDate = i.IssuedDate,
                    HotelName = i.Reservation.rooms.hotel!.Name,
                    StreetName = i.Reservation.rooms.hotel.Address,
                    Address = i.Reservation.rooms.hotel.City,
                    PaymentMethod = i.Payment!.PaymentMethod!.Name,

                    Services = i.Reservation.ReservationServices!
                    .Select(rs => new InvoiceServiceItem
                    {
                        ServiceName = rs.Service!.ServiceName,
                        Quantity = rs.Quantity.HasValue ? (int?)Convert.ToInt32(rs.Quantity.Value) : null,
                        Price = rs.TotalPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
