using Microsoft.EntityFrameworkCore;

namespace A6_Linq扩展.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CategoryName { get; set; }
    public decimal Price { get; set; }
    public string? Desc { get; set; }
}