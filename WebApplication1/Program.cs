using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DatabaseContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});
var app = builder.Build();

app.MapGet("/", () => "Hello World!");


app.MapPut("update-salary", async (int companyId, DatabaseContext dbContext) =>
{
    var company =await dbContext
        .Companies
        .Include(c => c.Employees)
        .FirstOrDefaultAsync(x => x.Id == companyId)!;
    if (company is null)
    {
        return Results.NotFound("company is null");
    }

    foreach (var employee in company.Employees)
    {
        employee.Salary = 1.1m * employee.Salary;
    }

    company.LastSalaryUpdateUtc = DateTime.UtcNow;
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});
app.Run();

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(builder =>
        {
            builder.HasMany(company => company.Employees)
                .WithOne()
                .HasForeignKey(employee => employee.CompanyId)
                .IsRequired();

            builder.HasData(new Company
            {
                Id = 1,
                Name = "Awesome Company",
            });
        });

        modelBuilder.Entity<Employee>(builder =>
        {
            var employees =
                Enumerable.Range(1, 1000)
                    .Select(id => new Employee
                    {
                        Id = id,
                        Name = $"emp #{id}",
                        Salary = 100,
                        CompanyId = 1
                    });
            builder.HasData(employees);
        });
    }
}

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? LastSalaryUpdateUtc { get; set; }
    public List<Employee> Employees { get; set; }
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int CompanyId { get; set; }
}