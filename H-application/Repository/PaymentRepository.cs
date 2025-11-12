using H_application.DTOs.Payment;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;


namespace H_Reservation.Service
{
    public class PaymentRepository : IPaymentService
    {
        private readonly EntityContext _context;
        private readonly IInvoicesServicecs _invoicesService;
        public PaymentRepository(EntityContext context, IInvoicesServicecs invoicesService)
        {
            _context = context;
            _invoicesService = invoicesService;
        }


        //public async Task<bool> CreatePaymentAsync(PaymentDtoCreate payment, CancellationToken cancellationToken)
        //{
        //    if (payment.ReservationId == null || !payment.ReservationId.Any())
        //        return false;

        //    using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        //    try
        //    {
        //        // Single reservation case
        //        if (payment.ReservationId.Count == 1)
        //        {
        //            var entity = new Payment
        //            {
        //                PaymentMethodId = payment.PaymentMethodId,
        //                PaymentDate = payment.PaymentDate,
        //                RefundDate = payment.RefundDate,
        //                RefundAmount = payment.RefundAmount,
        //                Currency = payment.Currency,
        //                Amount = payment.Amount,
        //                PaymentStatus = payment.PaymentStatus
        //            };

        //            await _context.Payments.AddAsync(entity, cancellationToken);
        //            await _context.SaveChangesAsync(cancellationToken);

        //            var reservation = await _context.Reservations
        //                .FirstOrDefaultAsync(r => r.ReservationId == payment.ReservationId.First(), cancellationToken);

        //            if (reservation != null)
        //                reservation.PaymentId = entity.PaymentId;

        //            await _context.SaveChangesAsync(cancellationToken);

        //            if (payment.PaymentStatus == "Completed")
        //                await _invoicesService.CreateInvoiceAsync(reservation!.ReservationId, cancellationToken);
        //        }
        //        else // Multiple reservations
        //        {
        //            // Make sure you have a matching list of amounts
        //            for (int i = 0; i < payment.ReservationId.Count; i++)
        //            {
        //                var reservationId = payment.ReservationId[i];
        //                var amountForReservation = payment.ReservationAmount[i]; // use actual amount for this reservation

        //                var entity = new Payment
        //                {
        //                    PaymentMethodId = payment.PaymentMethodId,
        //                    PaymentDate = payment.PaymentDate,
        //                    RefundDate = payment.RefundDate,
        //                    RefundAmount = payment.RefundAmount,
        //                    Currency = payment.Currency,
        //                    Amount = amountForReservation,
        //                    PaymentStatus = payment.PaymentStatus
        //                };

        //                await _context.Payments.AddAsync(entity, cancellationToken);
        //                await _context.SaveChangesAsync(cancellationToken);

        //                var reservation = await _context.Reservations
        //                    .FirstOrDefaultAsync(r => r.ReservationId == reservationId, cancellationToken);

        //                if (reservation != null)
        //                    reservation.PaymentId = entity.PaymentId;

        //                await _context.SaveChangesAsync(cancellationToken);

        //                if (payment.PaymentStatus == "Completed")
        //                    await _invoicesService.CreateInvoiceAsync(reservationId, cancellationToken);
        //            }
        //        }

        //        await transaction.CommitAsync(cancellationToken);
        //        return true;
        //    }
        //    catch
        //    {
        //        await transaction.RollbackAsync(cancellationToken);
        //        throw;
        //    }
        //}
        public async Task<bool> CreatePaymentAsync(PaymentDtoCreate payment, CancellationToken cancellationToken)
        {
            if (payment.ReservationId == null || !payment.ReservationId.Any())
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                if (payment.ReservationId.Count == 1)
                {
                    var reservationId = payment.ReservationId.First();
                    var reservation = await _context.Reservations
                        .Include(r => r.rooms)
                            .ThenInclude(rm => rm!.roomType)
                        .Include(r => r.ReservationServices)
                        .FirstOrDefaultAsync(r => r.ReservationId == reservationId, cancellationToken);

                    if (reservation == null)
                        return false;

                    // 🧮 Calculate total: room + extra services
                    var roomPrice = reservation.rooms?.roomType?.PricePerNight ?? 0;
                    var serviceTotal = reservation.ReservationServices?.Sum(rs => rs.TotalPrice ?? 0) ?? 0;
                    var totalAmount = roomPrice + serviceTotal;

                    var entity = new Payment
                    {
                        PaymentMethodId = payment.PaymentMethodId,
                        PaymentDate = payment.PaymentDate,
                        RefundDate = payment.RefundDate,
                        RefundAmount = payment.RefundAmount,
                        Currency = payment.Currency,
                        Amount = totalAmount,
                        PaymentStatus = payment.PaymentStatus
                    };

                    await _context.Payments.AddAsync(entity, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    reservation.PaymentId = entity.PaymentId;
                    await _context.SaveChangesAsync(cancellationToken);

                    if (payment.PaymentStatus == "Completed")
                        await _invoicesService.CreateInvoiceAsync(reservation.ReservationId, cancellationToken);
                }
                else
                {
                    for (int i = 0; i < payment.ReservationId.Count; i++)
                    {
                        var reservationId = payment.ReservationId[i];
                        var reservation = await _context.Reservations
                            .Include(r => r.rooms)
                                .ThenInclude(rm => rm!.roomType)
                            .Include(r => r.ReservationServices)
                            .FirstOrDefaultAsync(r => r.ReservationId == reservationId, cancellationToken);

                        if (reservation == null)
                            continue;

                        var roomPrice = reservation.rooms?.roomType?.PricePerNight ?? 0;
                        var serviceTotal = reservation.ReservationServices?.Sum(rs => rs.TotalPrice ?? 0) ?? 0;
                        var totalAmount = roomPrice + serviceTotal;

                        var entity = new Payment
                        {
                            PaymentMethodId = payment.PaymentMethodId,
                            PaymentDate = payment.PaymentDate,
                            RefundDate = payment.RefundDate,
                            RefundAmount = payment.RefundAmount,
                            Currency = payment.Currency,
                            Amount = totalAmount,
                            PaymentStatus = payment.PaymentStatus
                        };

                        await _context.Payments.AddAsync(entity, cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);

                        reservation.PaymentId = entity.PaymentId;
                        await _context.SaveChangesAsync(cancellationToken);

                        if (payment.PaymentStatus == "Completed")
                            await _invoicesService.CreateInvoiceAsync(reservationId, cancellationToken);
                    }
                }

                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }


        public async Task<bool> CreatePaymentForReservationAsync(PaymentDtoCreate dto, CancellationToken cancellationToken)
        {

            //var selectedReservation = await _context.Reservations
            //    .Include(r => r.rooms)
            //        .ThenInclude(rm => rm!.roomType)
            //    .FirstOrDefaultAsync(r => r.ReservationId == dto.ReservationId, cancellationToken);

            //if (selectedReservation == null)
            //    return false;


            //var relatedReservations = await _context.Reservations
            //    .Include(r => r.rooms)
            //        .ThenInclude(rm => rm!.roomType)
            //    .Where(r =>
            //        r.GuestId == selectedReservation.GuestId &&
            //        r.CheckInDate == selectedReservation.CheckInDate &&
            //        r.CheckOutDate == selectedReservation.CheckOutDate)
            //    .ToListAsync(cancellationToken);

            //if (!relatedReservations.Any())
            //    return false;


            //decimal totalAmount = relatedReservations.Sum(r => r.rooms?.roomType?.PricePerNight ?? 0);


            //var payment = new Payment
            //{
            //    PaymentMethodId = dto.PaymentMethodId,
            //    Amount = totalAmount,
            //    Currency = dto.Currency,
            //    RefundDate = dto.RefundDate,
            //    RefundAmount = dto.RefundAmount,
            //    PaymentStatus = dto.PaymentStatus,
            //    PaymentDate = DateTime.Now,
            //};

            //_context.Payments.Add(payment);
            //await _context.SaveChangesAsync(cancellationToken);


            //foreach (var res in relatedReservations)
            //{
            //    res.PaymentId = payment.PaymentId;
            //}

            //await _context.SaveChangesAsync(cancellationToken);
            //return true;
            return true;
        }


        public async Task<bool> DeletePaymentAsync(PaymentDtoUpdate payment, CancellationToken cancellation)
        {
            var entity = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == payment.PaymentId, cancellation);

            if (entity == null)
                return false;

            _context.Payments.Remove(entity);

            return await _context.SaveChangesAsync(cancellation) > 0;
        }

        public async Task<IEnumerable<PaymentResponse>> GetAllPaymentResponsesAsync(CancellationToken cancellationToken)
        {
            var payments = await _context.Payments
    .Include(p => p.PaymentMethod)
    .Include(p => p.Reservation!)
        .ThenInclude(r => r.guest)
    .Include(p => p.Reservation!)
        .ThenInclude(r => r.rooms)
    .AsNoTracking()
    .OrderBy(p => p.PaymentId)
    .ToListAsync(cancellationToken);

            var response = payments.Select(p => new PaymentResponse
            {
                PaymentId = p.PaymentId,
                PaymentMethodId = p.PaymentMethodId,
                Amount = p.Amount,
                Currency = p.Currency,
                TransactionId = p.TransactionId,
                PaymentStatus = p.PaymentStatus,
                PaymentDate = p.PaymentDate,
                RefundAmount = p.RefundAmount,
                RefundDate = p.RefundDate,
                PaymentMethodName = p.PaymentMethod?.Name ?? string.Empty,

                TotalPrice = p.Reservation?.Sum(r => r.TotalPrice ?? 0) ?? 0,

                GuestRoomInfo = (p.Reservation != null && p.Reservation.Any())
                    ? string.Join(" | ", p.Reservation.Select(r =>
                        $"Guest: {r.guest?.FirstName} {r.guest?.LastName} | Room: {r.rooms?.RoomNumber}"))
                    : string.Empty
            });


            return response;
        }


        public async Task<PaymentResponse> GetAllTotalPayment(string currency, CancellationToken cancellationToken)
        {
            var total = await _context.Payments
                .Where(p => p.Currency == currency && p.PaymentStatus == "Completed")
                .SumAsync(p => p.Amount, cancellationToken);

            return new PaymentResponse
            {
                TotalAmount = total ?? 0,
                Currency = currency
            };
        }

        //private async ValueTask<int> GetGuestIdFromReservationAsync(int reservationId, CancellationToken ct)
        //{
        //    var res = await _context.Reservations
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(r => r.ReservationId == reservationId, ct);
        //    return res?.GuestId ?? throw new Exception("Reservation not found");


        //}
        //private async ValueTask<DateTime> GetCheckInDateAsync(int reservationId, CancellationToken ct)
        //{
        //    var result = await _context.Reservations
        //        .AsNoTracking()
        //        .Where(r => r.ReservationId == reservationId)
        //        .Select(r => r.CheckInDate)
        //        .FirstOrDefaultAsync(ct);
        //    return result ?? throw new Exception("CheckInDate not found");
        //}

        private async ValueTask<DateTime> GetCheckOutDateAsync(int reservationId, CancellationToken ct)
        {
            var result = await _context.Reservations
                  .AsNoTracking()
                  .Where(r => r.ReservationId == reservationId)
                  .Select(r => r.CheckOutDate)
                  .FirstOrDefaultAsync(ct);
            return result ?? throw new Exception("CheckOut Not Found");
        }


        //public async Task<PaymentDtoUpdate?> GetPaymentByIdAsync(int id, CancellationToken cancellationToken)
        //{
        //    var payment = await _context.Payments
        //        .AsNoTracking()
        //        .Include(p => p.Reservation!)
        //            .ThenInclude(r => r.guest)
        //        .Include(p => p.Reservation!)
        //            .ThenInclude(r => r.rooms)
        //        .FirstOrDefaultAsync(p => p.PaymentId == id, cancellationToken);

        //    if (payment == null) return null;

        //    var dto = payment.Adapt<PaymentDtoUpdate>();


        //    dto.ReservationId = payment.Reservation?.Select(r => r.ReservationId).ToList();

        //    // Optional: create a friendly string with all guests/rooms
        //    if (payment.Reservation != null && payment.Reservation.Any())
        //    {
        //        dto.GuestRoomInfo = string.Join(" | ", payment.Reservation.Select(r =>
        //        {
        //            var guest = r.guest;
        //            var room = r.rooms;
        //            var total = r.TotalPrice;
        //            return $"Guest: {guest?.FirstName} {guest?.LastName ?? ""}, Room: {room?.RoomNumber ?? "N/A"}, Total: {total:C}";
        //        }));
        //    }
        //    else
        //    {
        //        dto.GuestRoomInfo = "No Reservations";
        //    }

        //    return dto;
        //}
        public async Task<PaymentDtoUpdate?> GetPaymentByIdAsync(int id, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments
                .AsNoTracking()
                .Include(p => p.Reservation!)
                    .ThenInclude(r => r.guest)
                .Include(p => p.Reservation!)
                    .ThenInclude(r => r.rooms)
                        .ThenInclude(rm => rm!.roomType)
                .Include(p => p.Reservation!)
                    .ThenInclude(r => r.ReservationServices) // Include extra services
                        .ThenInclude(rs => rs.Service)
                .FirstOrDefaultAsync(p => p.PaymentId == id, cancellationToken);

            if (payment == null) return null;

            var dto = payment.Adapt<PaymentDtoUpdate>();

            dto.ReservationId = payment.Reservation?.Select(r => r.ReservationId).ToList();

            // Calculate total including services
            decimal totalAmount = 0;
            if (payment.Reservation != null && payment.Reservation.Any())
            {
                var guestRoomInfoList = new List<string>();

                foreach (var res in payment.Reservation)
                {
                    var roomPrice = res.rooms?.roomType?.PricePerNight ?? 0;
                    var serviceTotal = res.ReservationServices?.Sum(rs => rs.TotalPrice ?? 0) ?? 0;
                    var subtotal = roomPrice + serviceTotal;

                    totalAmount += subtotal;

                    var guestName = res.guest != null ? $"{res.guest.FirstName} {res.guest.LastName}" : "Unknown Guest";
                    var roomNumber = res.rooms?.RoomNumber ?? "N/A";

                    guestRoomInfoList.Add($"Guest: {guestName}, Room: {roomNumber}, Total: {subtotal:C}");
                }

                dto.Amount = totalAmount;
                dto.GuestRoomInfo = string.Join(" | ", guestRoomInfoList);
            }
            else
            {
                dto.GuestRoomInfo = "No Reservations";
                dto.Amount = payment.Amount;
            }

            return dto;
        }


        public async Task<int> GetPaymentsCountByStatusAsync(string status, CancellationToken cancellationToken)
        {
            var count = await _context.Payments
                .Where(p => p.PaymentStatus == status)
                .CountAsync(cancellationToken);

            return count;
        }

        public async Task<bool> UpdatePaymentAsync(PaymentDtoUpdate payment, CancellationToken cancellationToken)
        {
            var entity = payment.Adapt<Payment>();


            var tracked = await _context.Payments.FindAsync(new object[] { entity.PaymentId }, cancellationToken);
            if (tracked != null)
            {
                _context.Entry(tracked).State = EntityState.Detached;
            }

            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }


        //public async Task<bool> UpdatePaymentWithInvoiceAsync(PaymentDtoUpdate dto, CancellationToken cancellation = default)
        //{

        //    var payment = await _context.Payments
        //        .Include(p => p.Reservation)
        //        .FirstOrDefaultAsync(p => p.PaymentId == dto.PaymentId, cancellation);

        //    if (payment == null)
        //        throw new Exception("Payment not found");


        //    payment.PaymentStatus = dto.PaymentStatus;
        //    payment.Amount = dto.Amount;
        //    payment.PaymentMethodId = dto.PaymentMethodId;
        //    payment.RefundAmount = dto.RefundAmount ?? payment.RefundAmount;
        //    payment.RefundDate = dto.RefundDate ?? payment.RefundDate;
        //    payment.PaymentDate = dto.PaymentDate ?? payment.PaymentDate;
        //    payment.Currency = dto.Currency ?? payment.Currency;

        //    _context.Payments.Update(payment);
        //    await _context.SaveChangesAsync(cancellation);


        //    if (payment.PaymentStatus == "Completed")
        //    {
        //        var linkedReservations = await _context.Reservations
        //            .Where(r => r.PaymentId == payment.PaymentId)
        //            .ToListAsync(cancellation);

        //        foreach (var reservation in linkedReservations)
        //        {

        //            var hasInvoice = await _context.Invoices
        //                .AnyAsync(i => i.ReservationId == reservation.ReservationId, cancellation);

        //            if (!hasInvoice)
        //            {
        //                await _invoicesService.CreateInvoiceAsync(reservation.ReservationId, cancellation);
        //            }
        //        }
        //    }

        //    return true;
        //}
        public async Task<bool> UpdatePaymentWithInvoiceAsync(PaymentDtoUpdate dto, CancellationToken cancellation = default)
        {
            var payment = await _context.Payments
                .Include(p => p.Reservation)
                    .ThenInclude(r => r.ReservationServices) // Include extra services
                .FirstOrDefaultAsync(p => p.PaymentId == dto.PaymentId, cancellation);

            if (payment == null)
                throw new Exception("Payment not found");

            // Update payment fields
            payment.PaymentStatus = dto.PaymentStatus;
            payment.PaymentMethodId = dto.PaymentMethodId;
            payment.RefundAmount = dto.RefundAmount ?? payment.RefundAmount;
            payment.RefundDate = dto.RefundDate ?? payment.RefundDate;
            payment.PaymentDate = dto.PaymentDate ?? payment.PaymentDate;
            payment.Currency = dto.Currency ?? payment.Currency;

            // Recalculate total including extra services
            decimal totalAmount = 0;
            if (payment.Reservation != null && payment.Reservation.Any())
            {
                foreach (var res in payment.Reservation)
                {
                    var roomPrice = res.rooms?.roomType?.PricePerNight ?? 0;
                    var serviceTotal = res.ReservationServices?.Sum(rs => rs.TotalPrice ?? 0) ?? 0;
                    totalAmount += roomPrice + serviceTotal;
                }
            }
            payment.Amount = totalAmount;

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync(cancellation);

            // Create invoices if completed
            if (payment.PaymentStatus == "Completed")
            {
                var linkedReservations = await _context.Reservations
                    .Where(r => r.PaymentId == payment.PaymentId)
                    .ToListAsync(cancellation);

                foreach (var reservation in linkedReservations)
                {
                    var hasInvoice = await _context.Invoices
                        .AnyAsync(i => i.ReservationId == reservation.ReservationId, cancellation);

                    if (!hasInvoice)
                        await _invoicesService.CreateInvoiceAsync(reservation.ReservationId, cancellation);
                }
            }

            return true;
        }


    }

}
