using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Foundation;
using Infrastructure.Foundation.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

builder.Services.AddDbContext<HotelManagementDbContext>( options =>
    options.UseSqlServer( builder.Configuration.GetConnectionString( "HotelManagement" ) ) );

builder.Services.AddScoped<IPropertyRepository, EFPropertyRepository>();
builder.Services.AddScoped<IRoomTypeRepository, EFRoomTypeRepository>();
builder.Services.AddScoped<IReservationRepository, EFReservationRepository>();

builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

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

app.MapControllers();

app.Run();