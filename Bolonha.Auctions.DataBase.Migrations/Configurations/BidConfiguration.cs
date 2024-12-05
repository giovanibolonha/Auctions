using Bolonha.Auctions.DataBase.Migrations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bolonha.Auctions.DataBase.Migrations.Configurations;

public class BidConfiguration
    : IEntityTypeConfiguration<BidData>
{
    public void Configure(EntityTypeBuilder<BidData> builder)
    {
        builder.ToTable("Bids");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Amount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(b => b.TimeOfBid)
               .IsRequired();

        builder.Property(b => b.Status)
               .IsRequired();

        builder.HasOne(b => b.Auction)
               .WithMany(a => a.Bids)
               .HasForeignKey(b => b.AuctionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
