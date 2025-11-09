using H_application.DTOs.ReservationDto;
using H_application.DTOs.RoomDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace H_Reservation.Service
{
    public class ReservationRepository : IReservationService
    {
        private readonly EntityContext _context;
        public ReservationRepository(EntityContext context)
        {
            _context = context;
        }

        public async Task<ReservationResponse> GetCheckInTrendAsync(CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var lastWeekStart = startOfWeek.AddDays(-7);
            var lastWeekEnd = startOfWeek.AddDays(-1);

            var thisWeekCheckIns = await _context.Reservations
                .Where(r => r.Status == "CheckedIn" && r.CheckInDate >= startOfWeek && r.CheckInDate <= today)
                .CountAsync(cancellationToken);
            var lastWeekCheckIns = await _context.Reservations
                  .Where(r => r.Status == "CheckedIn" && r.CheckInDate >= lastWeekStart && r.CheckInDate <= lastWeekEnd)
                  .CountAsync(cancellationToken);
            decimal growth = 0;
            if (lastWeekCheckIns > 0)
            {
                growth = ((decimal)(thisWeekCheckIns - lastWeekCheckIns) / lastWeekCheckIns) * 100;
            }
            return new ReservationResponse
            {
                CurrentMonthTotal = thisWeekCheckIns,
                GrowthPercentage = Math.Round(growth, 2)
            };
        }

       
        public async Task<bool> CreateReservationAsync(ReservationDtoCreate reservation, CancellationToken cancellationToken)
        {
            if (reservation.RoomId == null || !reservation.RoomId.Any())
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
               
                decimal? perRoomPrice = reservation.TotalPrice / reservation.RoomId.Count;

                foreach (var roomId in reservation.RoomId)
                {
                    var entity = new Reservation
                    {
                        RoomId = roomId,
                        Status = reservation.Status,
                        CheckInDate = reservation.CheckInDate,
                        CheckOutDate = reservation.CheckOutDate,
                        GuestId = reservation.GuestId,
                        Currency = reservation.Currency,
                        TotalPrice = perRoomPrice, 
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.Reservations.AddAsync(entity, cancellationToken);

                    var room = await _context.Rooms.FindAsync(new object[] { roomId }, cancellationToken);
                    if (room != null)
                    {
                        room.Status = reservation.Status switch
                        {
                            "Confirmed" => "Occupied",
                            "Pending" => "Reserved",
                            "Cancelled" => "Available",
                            _ => room.Status
                        };
                        _context.Rooms.Update(room);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return false;
            }
        }


        public async Task<bool> DeleteReservationAsync(ReservationDtoUpdate reservation, CancellationToken cancellationToken)
        {
            // Retrieve the entity from the database
            var entity = await _context.Reservations
                .FirstOrDefaultAsync(r => r.ReservationId == reservation.ReservationId, cancellationToken);

            if (entity == null) return false;

            _context.Reservations.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }


        public async Task<IEnumerable<ReservationResponse>> GetAllReservationAsync(string filter, CancellationToken cancellationToken)
        {
            var reserv = await _context.Reservations
                .OrderBy(g => g.CheckInDate)
                .Include(g => g.guest)
                .Include(r => r.rooms)
                .ThenInclude(rt => rt!.roomType)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var grouped = reserv
                .GroupBy(r => r.ReservationId)  
                .Select(g => new ReservationResponse
                {
                    ReservationId = g.Key,
                    GuestId = g.First().GuestId,
                    CheckInDate = g.First().CheckInDate,
                    CheckOutDate = g.First().CheckOutDate,
                    TotalPrice = g.Sum(x => x.TotalPrice ?? 0m),
                    Currency = g.First().Currency,
                    Status = g.First().Status,
                    FirstName = g.First().guest?.FirstName,
                    LastName = g.First().guest?.LastName,
                    Rooms = g.Select(x => new RoomResponse
                    {
                        RoomId = x.RoomId ?? 0,
                        RoomNumber = x.rooms?.RoomNumber,
                        RoomTypeName = x.rooms?.roomType?.Name,
                        RoomPrice = x.rooms?.roomType?.PricePerNight ?? 0,
                        RoomCurrency = x.rooms?.roomType?.Currency
                    }).ToList()
                });

            return grouped;
        }




        public async Task<ReservationDtoUpdate> GetReservationByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            
            var reservation = await _context.Reservations
                .Include(r => r.guest)
                .Include(r => r.rooms)  
                .ThenInclude(room => room!.roomType) 
                .FirstOrDefaultAsync(r => r.ReservationId == id, cancellationToken);

            if (reservation == null)
            {
                return null!; 
            }

           
            var bookingReservations = await _context.Reservations
                .Where(r => r.GuestId == reservation.GuestId
                            && r.CheckInDate == reservation.CheckInDate  
                            && r.CheckOutDate == reservation.CheckOutDate) 
                .Include(r => r.rooms)  // Fixed: singular
                .ThenInclude(room => room!.roomType)
                .ToListAsync(cancellationToken);

            if (!bookingReservations.Any())
            {
                return null!;
            }

            
            var dto = new ReservationDtoUpdate
            {
                ReservationId = reservation.ReservationId,
                GuestId = reservation.GuestId,
                RoomId = bookingReservations.Select(r => r.RoomId ?? 0).ToList(), 
                SelectedRoomIds = bookingReservations.Select(r => r.RoomId ?? 0).ToList(),
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                Currency = reservation.Currency,
                TotalPrice = bookingReservations.Sum(r => r.TotalPrice ?? 0m),  
                Status = reservation.Status,
                CreatedAt = reservation.CreatedAt?.Date ?? DateTime.MinValue,

            };

            
            dto.RoomResponses = bookingReservations
                .Where(r => r.rooms != null)  
                .Select(r => new RoomResponse
                {
                    RoomId = r.rooms!.RoomId, 
                    RoomNumber = r.rooms.RoomNumber ?? string.Empty,
                    RoomTypeName = r.rooms.roomType?.Name ?? string.Empty, 
                    RoomPrice = r.rooms.roomType?.PricePerNight ?? 0m, 
                    Currency = r.rooms.roomType?.Currency ?? string.Empty,
                    Status = r.rooms.Status ?? string.Empty
                }).ToList();

            return dto;
        }


        public async Task<ReservationResponse> PendingReservatoin(CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var startOfweek = today.AddDays(-(int)today.DayOfWeek);
            var lastWeekstart = startOfweek.AddDays(-7);
            var lastWeekEnd = startOfweek.AddDays(-1);

            var ThisWeekPending = await _context.Reservations
                .Where(r => r.Status == "Pending")
                .CountAsync(cancellationToken);

            var LastWeekPending = await _context.Reservations
                .Where(r => r.Status == "Pending")
                .CountAsync(cancellationToken);

            decimal grow = 0;
            if (LastWeekPending > 0)
            {
                grow = ((decimal)(ThisWeekPending - LastWeekPending)) / LastWeekPending * 100;
            }
            return new ReservationResponse
            {
                CurrentMonthTotal = ThisWeekPending,
                GrowthPercentage = Math.Round(grow, 2)
            };

        }

        public async Task<ReservationResponse> TotalReservation(CancellationToken cancellationToken)
        {
            // 1️⃣ Get all reservations
            var reservations = await _context.Reservations
                .ToListAsync(cancellationToken);
            // 2️⃣ Group by month and year using CreatedAt
            var monthlyData = reservations
                .GroupBy(r => new { r.CreatedAt?.Year, r.CreatedAt?.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            // 3️⃣ Get current and previous month
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            var previousYear = currentMonth == 1 ? currentYear - 1 : currentYear;

            // 4️⃣ Find total reservations for both months
            var currentMonthData = monthlyData
                .FirstOrDefault(x => x.Month == currentMonth && x.Year == currentYear)?.Count ?? 0;

            var previousMonthData = monthlyData
                .FirstOrDefault(x => x.Month == previousMonth && x.Year == previousYear)?.Count ?? 0;

            // 5️⃣ Calculate growth
            decimal growthPercent = 0;
            if (previousMonthData > 0)
            {
                growthPercent = ((decimal)(currentMonthData - previousMonthData) / previousMonthData) * 100;
            }

            // 6️⃣ Return as a single summary response
            return new ReservationResponse
            {
                CurrentMonthTotal = currentMonthData,
                GrowthPercentage = Math.Round(growthPercent, 2)
            };
        }


        public async Task<bool> UpdateReservationAsync(ReservationDtoUpdate reservation, CancellationToken cancellationToken)
        {
            var entity = reservation.Adapt<Reservation>();
            _context.Reservations.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        
        public async Task<bool> UpdateReservationStatusAsync(ReservationDtoUpdate dto, CancellationToken cancellation)
        {
            if (dto.ReservationId <= 0 || dto.RoomId == null || !dto.RoomId.Any())
                return false;

            var anchorReservation = await _context.Reservations.FindAsync(dto.ReservationId);
            if (anchorReservation == null)
                return false;

           
            var existingReservations = await _context.Reservations
                .Where(r => r.GuestId == anchorReservation.GuestId
                            && r.CheckInDate == anchorReservation.CheckInDate
                            && r.CheckOutDate == anchorReservation.CheckOutDate)
                .ToListAsync(cancellation);

            var newRoomIds = new HashSet<int>(dto.RoomId);
            var existingRoomIds = new HashSet<int>(existingReservations.Select(r => r.RoomId ?? 0));
            var perRoomPrice = (dto.TotalPrice ?? 0m) / dto.RoomId.Count;

            using var transaction = await _context.Database.BeginTransactionAsync(cancellation);
            try
            {
                
                foreach (var roomIdToRemove in existingRoomIds.Except(newRoomIds))
                {
                    var oldRes = existingReservations.FirstOrDefault(r => r.RoomId == roomIdToRemove);
                    if (oldRes != null)
                    {
                        
                        var oldRoom = await _context.Rooms.FindAsync(roomIdToRemove);
                        if (oldRoom != null)
                        {
                            oldRoom.Status = "Available"; 
                            _context.Rooms.Update(oldRoom);
                        }

                       
                        _context.Reservations.Remove(oldRes);
                    }
                }
                foreach (var roomIdToUpdate in existingRoomIds.Intersect(newRoomIds))
                {
                    var oldRes = existingReservations.FirstOrDefault(r => r.RoomId == roomIdToUpdate);
                    if (oldRes != null)
                    {
                        oldRes.GuestId = dto.GuestId ?? oldRes.GuestId;
                        oldRes.CheckInDate = dto.CheckInDate ?? oldRes.CheckInDate;
                        oldRes.CheckOutDate = dto.CheckOutDate ?? oldRes.CheckOutDate;
                        oldRes.TotalPrice = perRoomPrice;
                        oldRes.Status = dto.Status ?? oldRes.Status;
                        oldRes.Currency = dto.Currency ?? oldRes.Currency;

                        _context.Reservations.Update(oldRes);

                        var room = await _context.Rooms.FindAsync(roomIdToUpdate);
                        if (room != null)
                        {
                            room.Status = GetRoomStatusFromReservationStatus(oldRes.Status!, oldRes.CheckOutDate);
                            _context.Rooms.Update(room);
                        }
                    }
                }

               
                foreach (var roomIdToAdd in newRoomIds.Except(existingRoomIds))
                {
                    var newRes = new Reservation
                    {
                        GuestId = dto.GuestId ?? anchorReservation.GuestId,
                        RoomId = roomIdToAdd,
                        CheckInDate = dto.CheckInDate ?? anchorReservation.CheckInDate,
                        CheckOutDate = dto.CheckOutDate ?? anchorReservation.CheckOutDate,
                        TotalPrice = perRoomPrice,
                        Status = dto.Status ?? anchorReservation.Status,
                        Currency = dto.Currency ?? anchorReservation.Currency,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _context.Reservations.AddAsync(newRes, cancellation);

                    var room = await _context.Rooms.FindAsync(roomIdToAdd);
                    if (room != null)
                    {
                        room.Status = GetRoomStatusFromReservationStatus(newRes.Status!, newRes.CheckOutDate);
                        _context.Rooms.Update(room);
                    }
                }

                await _context.SaveChangesAsync(cancellation);
                await transaction.CommitAsync(cancellation);
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellation);
                return false;
            }
        }

        private string GetRoomStatusFromReservationStatus(string resStatus, DateTime? checkOutDate)
        {
            return resStatus switch
            {
                "Confirmed" => "Occupied",
                "CheckedIn" => "Occupied",
                "Pending" => "Reserved",
                "CheckedOut" => "Cleaning",
                "Cancelled" => "Available",
                _ => "Available"
            };
        }

        public async Task<ReservationResponse> ConfirmReservationAsync(CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var lastWeekStart = startOfWeek.AddDays(-7);
            var lastWeekEnd = startOfWeek.AddDays(-1);

            var thisWeekCheckIns = await _context.Reservations
                .Where(r => r.Status == "Confirmed" && r.CheckInDate >= startOfWeek && r.CheckInDate <= today)
                .CountAsync(cancellationToken);
            var lastWeekCheckIns = await _context.Reservations
                  .Where(r => r.Status == "Confirmed" && r.CheckInDate >= lastWeekStart && r.CheckInDate <= lastWeekEnd)
                  .CountAsync(cancellationToken);
            decimal growth = 0;
            if (lastWeekCheckIns > 0)
            {
                growth = ((decimal)(thisWeekCheckIns - lastWeekCheckIns) / lastWeekCheckIns) * 100;
            }
            return new ReservationResponse
            {

                CurrentMonthTotal = thisWeekCheckIns,
                GrowthPercentage = Math.Round(growth, 2)

            };
        }

        public async Task<List<MonthlyRevenueDto>> GetMonthlyRevenueByCurrencyAsync(string currency, int year, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be null or empty.", nameof(currency));

            var payments = await _context.Payments
                .Where(p => p.Currency == currency && p.PaymentStatus == "Completed" &&
                p.PaymentDate.HasValue &&
                p.PaymentDate.Value.Year == year).ToListAsync();

            var monthlyRevenue = payments
                .GroupBy(p => new { p.PaymentDate!.Value.Year, p.PaymentDate.Value.Month })
               .Select(g => new MonthlyRevenueDto
               {
                   Year = g.Key.Year,
                   Month = g.Key.Month,
                   TotalRevenue = g.Sum(p => p.Amount ?? 0)
               }).OrderBy(x => x.Month).ToList();
            for (int i = 0; i < monthlyRevenue.Count; i++)
            {
                if (i == 0)
                {
                    monthlyRevenue[i].GrowthPercentage = 0;
                }
                else
                {
                    var previousMonth = monthlyRevenue[i - 1].TotalRevenue;
                    var currentMonth = monthlyRevenue[i].TotalRevenue;
                    monthlyRevenue[i].GrowthPercentage = previousMonth == 0
                ? 100 
                : Math.Round(((currentMonth - previousMonth) / previousMonth) * 100, 2);
                }
            }
            return monthlyRevenue;

        }

        public async Task<List<RoomCalendarDto>> GetRoomCalendarAsync(DateTime? startDate, CancellationToken cancellationToken)
        {
            var start = startDate ?? DateTime.Today;
            var end = start.AddDays(30); // 1-month view

            var reservations = await _context.Reservations
                .Include(r => r.rooms)
                .ThenInclude(rt => rt!.roomType)
                .Include(r => r.guest)
                .Where(r => r.CheckInDate != null && r.CheckInDate <= end &&
                            (r.CheckOutDate == null || r.CheckOutDate >= start))
                .ToListAsync(cancellationToken);

            var calendarData = new List<RoomCalendarDto>();

            foreach (var res in reservations)
            {
                if (res.rooms == null) continue;

                var guestName = res.guest != null
                    ? $"{res.guest.FirstName} {res.guest.LastName}"
                    : "Unknown";

                var checkIn = res.CheckInDate ?? start;
                var checkOut = res.CheckOutDate ?? end;


                var room = res.rooms;
                if (room == null) continue;

                for (var date = checkIn.Date; date <= checkOut.Date; date = date.AddDays(1))
                {
                    if (date < start || date > end) continue;

                    calendarData.Add(new RoomCalendarDto
                    {
                        ReservationId = res.ReservationId,
                        Date = date,
                        RoomNumber = room.RoomNumber,
                        RoomType = room.roomType?.Name ?? "Unknown",
                        GuestName = guestName,
                        Status = res.Status ?? "Available",
                        CheckInDate = res.CheckInDate,
                        CheckOutDate = res.CheckOutDate
                    });
                }
            }
            return calendarData;
        }
    }
}
