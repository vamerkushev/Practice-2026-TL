using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;

internal class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure( EntityTypeBuilder<RoomType> builder )
    {
        builder.ToTable( "RoomTypes" );
        builder.HasKey( r => r.Id );

        builder.Property( r => r.Name )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.Property( r => r.DailyPrice )
               .HasPrecision( 10, 2 )
               .IsRequired();

        builder.Property( r => r.Currency )
               .HasMaxLength( 3 )
               .IsRequired();

        builder.Property( r => r.MinPersonCount )
               .IsRequired();

        builder.Property( r => r.MaxPersonCount )
               .IsRequired();

        builder.Property( r => r.AvailableRoomsCount )
               .IsRequired();
    }
}