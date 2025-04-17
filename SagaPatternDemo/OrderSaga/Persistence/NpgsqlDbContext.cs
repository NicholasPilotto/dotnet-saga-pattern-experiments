using Microsoft.EntityFrameworkCore;

namespace OrderSaga.Persistence;

public class NpgsqlDbContext(DbContextOptions<NpgsqlDbContext> options) : DbContext(options)
{
}
