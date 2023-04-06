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
    #region 1.遍历更新 会产生1000个update语句

    // var company =await dbContext
    //     .Companies
    //     .Include(c => c.Employees)
    //     .FirstOrDefaultAsync(x => x.Id == companyId)!;
    // if (company is null)
    // {
    //     return Results.NotFound("company is null");
    // }
    //
    // foreach (var employee in company.Employees)
    // {
    //     employee.Salary = 1.1m * employee.Salary;
    // }
    //
    // company.LastSalaryUpdateUtc = DateTime.UtcNow;
    // await dbContext.SaveChangesAsync();

    #endregion

    #region 2.使用ef执行原生sql,开启事务保证完整性

    // var company = await dbContext
    //     .Companies
    //     .Include(c => c.Employees)
    //     .FirstOrDefaultAsync(x => x.Id == companyId)!;
    // if (company is null) return Results.NotFound("company is null");
    //
    // await dbContext.Database.BeginTransactionAsync();
    // await dbContext.Database.ExecuteSqlInterpolatedAsync(
    //     $"update Employees set Salary=Salary*1.1 where CompanyId={company.Id}");
    //
    // company.LastSalaryUpdateUtc = DateTime.UtcNow;
    // // await dbContext.SaveChangesAsync();
    // await dbContext.Database.CommitTransactionAsync();

    #endregion

    #region 3.使用Dapper

    // var company = await dbContext
    //     .Companies
    //     .Include(c => c.Employees)
    //     .FirstOrDefaultAsync(x => x.Id == companyId)!;
    // if (company is null) return Results.NotFound("company is null");
    //
    //
    // var transaction = await dbContext.Database.BeginTransactionAsync();
    // await dbContext.Database.GetDbConnection().ExecuteAsync(
    //     "update Employees set Salary=Salary*1.1 where CompanyId=@CompanyId",
    //     new { CompanyId = company.Id },
    //     transaction.GetDbTransaction()
    // );
    //
    // company.LastSalaryUpdateUtc = DateTime.UtcNow;
    // await dbContext.SaveChangesAsync();
    // await dbContext.Database.CommitTransactionAsync();

    #endregion


    #region 4.使用ef core 新增的批量操作

    var company = await dbContext
        .Companies
        .Include(c => c.Employees)
        .FirstOrDefaultAsync(x => x.Id == companyId)!;
    if (company is null) return Results.NotFound("company is null");

    // https://learn.microsoft.com/zh-cn/ef/core/what-is-new/ef-core-7.0/whatsnew
    var transaction = await dbContext.Database.BeginTransactionAsync();
    await dbContext.Employees.Where(c => c.Id == company.Id)
        .ExecuteUpdateAsync(
            s => s.SetProperty(e => e.Salary, e => e.Salary * 1.1m));

    company.LastSalaryUpdateUtc = DateTime.UtcNow;
    await dbContext.SaveChangesAsync();
    await dbContext.Database.CommitTransactionAsync();

    #endregion

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
                Name = "Awesome Company"
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