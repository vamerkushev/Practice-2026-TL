using Infrastructure.Foundation.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Foundation;

public class HotelManagementDbContext : DbContext
{
    public HotelManagementDbContext( DbContextOptions<HotelManagementDbContext> options )
            : base( options )
    {
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );

        modelBuilder.ApplyConfiguration( new PropertyConfiguration() );
        modelBuilder.ApplyConfiguration( new RoomTypeConfiguration() );
        modelBuilder.ApplyConfiguration( new ReservationConfiguration() );
    }
}