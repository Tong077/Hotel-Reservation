

using H_Domain.Configuration;
using H_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace H_Domain.DataContext
{
    public class EntityContext : DbContext
    {
        public EntityContext(DbContextOptions<EntityContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder optionsBuilder)
        {
            base.OnModelCreating(optionsBuilder);
            optionsBuilder.ApplyConfiguration(new HotelsConfig());
            optionsBuilder.ApplyConfiguration(new GuestConfig());
            optionsBuilder.ApplyConfiguration(new PaymentConfig());
            optionsBuilder.ApplyConfiguration(new PaymentMethodConfig());
            optionsBuilder.ApplyConfiguration(new ReservationConfig());
            optionsBuilder.ApplyConfiguration(new RoomConfig());
            optionsBuilder.ApplyConfiguration(new RoomTypeConfig());

            optionsBuilder.ApplyConfiguration(new EmployeesConfiguration());
            optionsBuilder.ApplyConfiguration(new HousekeepingConfiguration());
            optionsBuilder.ApplyConfiguration(new ServicesConfiguration());
            optionsBuilder.ApplyConfiguration(new ReservationServicesConfiguration());
            optionsBuilder.ApplyConfiguration(new BookingHistoryConfiguration());
            optionsBuilder.ApplyConfiguration(new InvoicesConfiguration());
            optionsBuilder.ApplyConfiguration(new ReviewsConfiguration());
            optionsBuilder.ApplyConfiguration(new SystemSettingsConfiguraion());
            optionsBuilder.ApplyConfiguration(new ReservationRoomsConfiguration());

        }


        public DbSet<Guest> Guests { get; set; }
        public DbSet<Hotels> Hotels { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<ReservationRooms> ReservationRooms { get; set; }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Housekeeping> Housekeeping { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<ReservationService> ReservationsService { get; set; }
        public DbSet<BookingHistory> BookingHistory { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }

    }
}
