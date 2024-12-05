using Bolonha.Auctions.DataBase.Migrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bolonha.Auctions.DataBase.Migrations.Configurations;

public class AuctionConfiguration 
    : IEntityTypeConfiguration<AuctionData>
{
    public void Configure(EntityTypeBuilder<AuctionData> builder)
    {
        builder.ToTable("Auctions");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(a => a.StartingPrice)
               .HasColumnType("decimal(18,2)");

        builder.Property(a => a.Status)
               .IsRequired(); 

        builder.Property(a => a.StartTime)
               .IsRequired();

        builder.Property(a => a.EndTime)
               .IsRequired();

        builder.HasMany(a => a.Bids)
               .WithOne(b => b.Auction)
               .HasForeignKey(b => b.AuctionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.WinningBid)
               .WithMany()
               .HasForeignKey(a => a.WinningBidId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.EndTime)
           .HasDatabaseName("IX_Auctions_EndTime");
    }
}
