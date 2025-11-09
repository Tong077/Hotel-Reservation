using H_application.DTOs.ReservationDto;
using H_application.DTOs.ReservationServicesDto;
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
    public class ReservationServicesRepositoy : IReservationServicesService
    {
        private readonly EntityContext _context;

        public ReservationServicesRepositoy(EntityContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReservationService(ReservationServicesDtoCreate dto, CancellationToken cancellationToken = default)
        {

            var entity = dto.Adapt<ReservationService>();
            await _context.ReservationsService.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteReservationService(ReservationServicesDtoUpdate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<ReservationService>();
            _context.ReservationsService.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<ReservationServicesResponse>> GetAllReservationService(CancellationToken cancellationToken = default)
        {
            var reser = await _context.ReservationsService
                .Include(x => x.Reservation)
                .Include(x => x.Service)
                .Include(x => x.Reservation!.rooms)
                .ThenInclude(r => r.roomType)
                .Include(x => x.Reservation!.guest)
                .AsNoTracking()
                .ToListAsync();

            var entity = reser.Select(x => new ReservationServicesResponse
            {
                ReservationServiceId = x.ReservationServiceId,
                ReservationId = x.ReservationId,
                ServiceId = x.ServiceId,
                Quantity = x.Quantity,
                TotalPrice = x.TotalPrice,
                GuestId = x.Reservation!.GuestId ?? 0,
                RoomId = x.Reservation.RoomId ?? 0,
                FirstName = x.Reservation!.guest!.FirstName,
                LastName = x.Reservation.guest.LastName,

                RoomNumber = x.Reservation.rooms?.RoomNumber,
                RoomTypeName = x.Reservation.rooms?.roomType?.Name,
                CheckIn = x.Reservation.CheckInDate,
                CheckOut = x.Reservation.CheckOutDate,
                BasePrice = x.Reservation.TotalPrice,
                ServiceName = x.Service!.ServiceName,
                ServicePrice = x.Service.Price,
            }).ToList();
            return entity;
        }

        public async Task<ReservationServicesDtoUpdate> GetReservationById(int Id, CancellationToken cancellationToken = default)
        {
            var reserv = await _context.ReservationsService
                .FirstOrDefaultAsync(r => r.ReservationServiceId == Id, cancellationToken);
            var entity = reserv?.Adapt<ReservationServicesDtoUpdate>();
            return entity!;
        }

        public async Task<bool> UpdateReservationService(ReservationServicesDtoUpdate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<ReservationService>();
            _context.ReservationsService.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        
    }
}
