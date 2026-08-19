using Domain.Interfaces.Repositories;
using Infrastructure.Foundation;
using Infrastructure.Foundation.Repositories;
using Microsoft.EntityFrameworkCore;
using Application.Services;
using WebApi2.ExceptionHandler;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

builder.Services.AddDbContext<HotelManagementDbContext>( options =>
    options.UseSqlServer( builder.Configuration.GetConnectionString( "HotelManagement" ) ) );

builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IPropertyRepository, EFPropertyRepository>();
builder.Services.AddScoped<IRoomTypeRepository, EFRoomTypeRepository>();
builder.Services.AddScoped<IReservationRepository, EFReservationRepository>();

builder.Services.AddScoped<PropertyService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<RoomTypeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();