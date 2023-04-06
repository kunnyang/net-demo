using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication1;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region 使用配置管理参数
// 这里也可以直接绑定参数 加了一层DatabaseOptionsSetup 包含了DatabaseOptions节点和ConnectionStrings节点
// builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.Database));
builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
builder.Services.AddDbContext<DatabaseContext>((serviceProvider, dbContextOptionsBuilder) =>
{
    var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;
    dbContextOptionsBuilder.UseSqlServer(databaseOptions.ConnectionStrings,
        sqlServerOptionsAction =>
        {
            sqlServerOptionsAction.EnableRetryOnFailure(databaseOptions.MaxRetryOnFailure);
            sqlServerOptionsAction.CommandTimeout(databaseOptions.CommandTimeout);
        });
    dbContextOptionsBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
    dbContextOptionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
});

#endregion


#region 使用硬编码的方式 不易管理 调整的时候必须修改代码

// builder.Services
//     .AddDbContext<DatabaseContext>(dbContextOptionsBuilder =>
//     {
//         var connectionString = builder.Configuration.GetConnectionString("default");
//         dbContextOptionsBuilder.UseSqlServer(connectionString, sqlserverAction =>
//         {
//             //失败重试
//             sqlserverAction.EnableRetryOnFailure(3);
//             //超时时间
//             sqlserverAction.CommandTimeout(30);
//         });
//         // 显示详细错误信息
//         dbContextOptionsBuilder.EnableDetailedErrors();
//         // 允许将应用程序数据包含在异常消息、日志记录等中。
//         dbContextOptionsBuilder.EnableSensitiveDataLogging();
//     });

#endregion


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var scopeServiceProvider = scope.ServiceProvider;
    var dbContext = scopeServiceProvider.GetRequiredService<DatabaseContext>();
    var logger = scopeServiceProvider.GetRequiredService<ILogger<Program>>();
    dbContext.Database.EnsureCreated();
    logger.LogInformation("Database create");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();