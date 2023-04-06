namespace WebApplication1;

public class DatabaseOptions
{
    public const string Database = "DatabaseOptions";
    public string? ConnectionStrings { get; set; }
    public int MaxRetryOnFailure { get; set; }
    public int CommandTimeout { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }
}