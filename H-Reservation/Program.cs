using H_application.Error;
using H_application.Repository;
using H_application.Service;
using H_Application.Repository;
using H_Application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using H_Reservation.Feature;
using H_Reservation.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rotativa.AspNetCore;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddFilter("Microsoft.WebTools.BrowserLink.Net", LogLevel.Error);
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("MyConnection");
builder.Services.AddDbContext<EntityContext>(options =>
options.UseSqlServer(connectionString));



builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<EntityContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        )

        };
    });
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
builder.Services.AddScoped<IPermissionService, PermissoinRepository>();
builder.Services.AddScoped<IRoleService, RoleRepository>();
builder.Services.AddScoped<IImageUploadsService, UploadImageHandler>();

var app = builder.Build();
var env = builder.Environment;


RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.Run();
