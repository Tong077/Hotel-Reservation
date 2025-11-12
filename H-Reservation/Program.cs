using H_application.Error;
using H_application.Repository;
using H_application.Service;
using H_Application.Service;
using H_Domain.DataContext;
using H_Reservation.Service;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddFilter("Microsoft.WebTools.BrowserLink.Net", LogLevel.Error);
builder.Services.AddControllersWithViews();

// connection string  register //
var connectionString = builder.Configuration.GetConnectionString("MyConnection");
builder.Services.AddDbContext<EntityContext>(options =>
options.UseSqlServer(connectionString));


builder.Services.AddScoped<IGustService, GuestRepositry>();
builder.Services.AddScoped<IHotelServicecs, HotelRepository>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodRepository>();
builder.Services.AddScoped<IPaymentService, PaymentRepository>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IRoomService, RoomRepository>();
builder.Services.AddScoped<IReservationService, ReservationRepository>();
builder.Services.AddScoped<ISystemSettingsService, SystemSettingsRepository>();
builder.Services.AddScoped<IEmployeesService, EmployeesRepository>();
builder.Services.AddScoped<IHousekeepingService, HousekeepingRepository>();
builder.Services.AddScoped<IServicesService, ServicesRepository>();
builder.Services.AddScoped<IReservationServicesService, ReservationServicesRepositoy>();
builder.Services.AddScoped<IBookingHistoryService, BookingHistoryRepository>();
builder.Services.AddScoped<IInvoicesServicecs, InvoicesRepository>();
builder.Services.AddScoped<IReviewsService, ReviewsRepositoy>();

var app = builder.Build();
var env = builder.Environment;


RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.Run();
