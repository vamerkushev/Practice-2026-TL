using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Foundation.Configurations;

internal class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure( EntityTypeBuilder<Property> builder )
    {
        builder.ToTable( "Properties" );
        builder.HasKey( p => p.Id );

        builder.Property( p => p.Name )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.Property( p => p.Country )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.Property( p => p.City )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.Property( p => p.Address )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.HasMany( p => p.RoomTypes )
               .WithOne( r => r.Property )
               .HasForeignKey( r => r.PropertyId )
               .OnDelete( DeleteBehavior.Restrict );
    }
}