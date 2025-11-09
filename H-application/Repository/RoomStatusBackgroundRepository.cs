using H_application.Service;
using H_Domain.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Repository
{
    public class RoomStatusBackgroundRepository : IBackgroundService
    {

        private readonly IServiceScopeFactory _scopeFactory;
        public RoomStatusBackgroundRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scrope = _scopeFactory.CreateScope();
                var context = scrope.ServiceProvider.GetRequiredService<EntityContext>();

                var reservationsToClean = await context.Reservations
                    .Where(r => r.Status == "CheckedOut" && r.CheckOutDate <= DateTime.Now)
                    .ToListAsync(stoppingToken);

                foreach (var res in reservationsToClean)
                {
                    var room = await context.Rooms.FindAsync(res.RoomId);
                    if(room != null && room.Status == "Cleaning"){
                        room.Status = "Cleaning";
                        context.Rooms.Update(room);
                    };
                }
                await context.SaveChangesAsync();

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }

        }
    }
}
