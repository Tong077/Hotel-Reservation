using H_application.DTOs.GuestDto;
using H_Application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;

namespace H_application.Service
{
    public class GuestRepositry : IGustService
    {
        private readonly EntityContext? context;
        private readonly ILogger logger;

        public GuestRepositry(EntityContext? context, ILogger<GuestRepositry> logger)
        {
            this.context = context;
            this.logger = logger;

        }
        public async Task<GuestDtoUpdate> GetById(int guestId)
        {
            var guest = await context!.Guests
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GuestId == guestId);
            if (guest == null)
                return null;
            var dto = guest.Adapt<GuestDtoUpdate>();


            return dto;
        }

        public async Task<bool> guestCreateAsync(GuestDtoCreate create, CancellationToken cancellationToken)
        {
            var entity = create.Adapt<Guest>();

           context!.Guests.Add(entity);
            return await context.SaveChangesAsync() > 0;
        }


        public async Task<bool> guestDeleteAsync(GuestDtoUpdate delete, CancellationToken cancellationToken)
        {
            var entity = delete.Adapt<Guest>();
            context!.Guests.Remove(entity);
            return await context.SaveChangesAsync() > 0;

        }

        public async Task<bool> guestUpdateAsync(GuestDtoUpdate update, CancellationToken cancellationToken)
        {
            var entity = update.Adapt<Guest>();
            context!.Guests.Update(entity);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<GuestResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            var guests = await context!.Guests
                .OrderBy(g=> g.FirstName)
                .OrderBy(g => g.LastName)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return guests.Select(g => g.Adapt<GuestResponse>()).ToList();
        }

        

        public async Task<(bool Success, int GuestId, string? ErrorType)> guestCreatsAsync(GuestDtoCreate create, CancellationToken cancellationToken)
        {
            var entity = create.Adapt<Guest>();

            // Check for email duplicate first
            var emailExists = await context!.Guests
                .AnyAsync(g => g.Email == entity.Email && !string.IsNullOrEmpty(entity.Email), cancellationToken);

            if (emailExists)
            {
                return (false, 0, "Email");
            }

            // Then check for phone duplicate
            var phoneExists = await context!.Guests
                .AnyAsync(g => g.Phone == entity.Phone && !string.IsNullOrEmpty(entity.Phone), cancellationToken);

            if (phoneExists)
            {
                return (false, 0, "Phone");
            }

            try
            {
                await context.AddAsync(entity, cancellationToken);
                var saveResult = await context.SaveChangesAsync(cancellationToken);
                if (saveResult > 0)
                {
                    return (true, entity.GuestId, null); // Assuming Guest has an auto-generated int Id
                }
                return (false, 0, null); // Save failed without specific error
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Save error: {ex.InnerException?.Message}");
                return (false, 0, null);
            }
        }
    }
}
