using Microsoft.EntityFrameworkCore;
using OrderService.Entities;

namespace OrderService.Persistence;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Order>().HasKey(e => e.Id);
  }
  public DbSet<Order> Orders { get; set; }
}