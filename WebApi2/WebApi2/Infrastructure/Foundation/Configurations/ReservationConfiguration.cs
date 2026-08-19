using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;

internal class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure( EntityTypeBuilder<Reservation> builder )
    {
        builder.ToTable( "Reservations" );
        builder.HasKey( r => r.Id );

        builder.Property( r => r.GuestName )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.Property( r => r.GuestPhoneNumber )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.Property( r => r.Total )
               .HasPrecision( 10, 2 )
               .IsRequired();

        builder.Property( r => r.Currency )
               .HasMaxLength( 3 )
               .IsRequired();

        builder.Property( r => r.ArrivalTime )
               .HasMaxLength( 10 )
               .IsRequired();

        builder.Property( r => r.DepartureTime )
               .HasMaxLength( 10 )
               .IsRequired();

        builder.HasOne( r => r.Property )
               .WithMany()
               .HasForeignKey( r => r.PropertyId )
               .OnDelete( DeleteBehavior.NoAction );

        builder.HasOne( r => r.RoomType )
               .WithMany()
               .HasForeignKey( r => r.RoomTypeId )
               .OnDelete( DeleteBehavior.NoAction );
    }
}