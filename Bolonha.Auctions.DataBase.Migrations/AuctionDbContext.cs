using Bolonha.Auctions.DataBase.Migrations.Configurations;
using Bolonha.Auctions.DataBase.Migrations.Models;
using Microsoft.EntityFrameworkCore;

namespace Bolonha.Auctions.DataBase.Migrations;

public class AuctionDbContext(DbContextOptions<AuctionDbContext> options) 
    : DbContext(options)
{
    public virtual required DbSet<AuctionData> Auctions { get; set; }
    public virtual required DbSet<BidData> Bids { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuctionConfiguration());
        modelBuilder.ApplyConfiguration(new BidConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
