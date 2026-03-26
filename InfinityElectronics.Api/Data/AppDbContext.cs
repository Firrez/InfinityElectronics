using InfinityElectronics.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace InfinityElectronics.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}
