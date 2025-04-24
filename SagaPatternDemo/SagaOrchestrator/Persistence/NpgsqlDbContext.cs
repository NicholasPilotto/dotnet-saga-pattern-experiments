using Microsoft.EntityFrameworkCore;

namespace SagaOrchestrator.Persistence;

public class NpgsqlDbContext(DbContextOptions<NpgsqlDbContext> options)
  : DbContext(options)
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
  }
}