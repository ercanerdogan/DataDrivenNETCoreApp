using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopAdmin.Models;

public class BethanyPieShopDbContext : DbContext
{
    public BethanyPieShopDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Pie> Pies { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BethanyPieShopDbContext).Assembly);
    }
}