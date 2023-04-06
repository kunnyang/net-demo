using Microsoft.Extensions.Options;

namespace WebApplication1;
/*
 * 使用IConfigureOptions 可以绑定不在一个节点的配置 到一个class里
 * ConnectionStrings不在DatabaseOptions节点
 * builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
 */
public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DatabaseOptionsSetup> _logger;

    public DatabaseOptionsSetup(IConfiguration configuration, ILogger<DatabaseOptionsSetup> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void Configure(DatabaseOptions options)
    {
        _logger.LogInformation("add DatabaseOptions ConnectionString");
        options.ConnectionStrings = _configuration.GetConnectionString("default");
        _logger.LogInformation("add DatabaseOptions Section");
        _configuration.GetSection(DatabaseOptions.Database).Bind(options);
    }
}