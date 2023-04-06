using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly ILogger<StudentController> _logger;
    private readonly DatabaseOptions _databaseOptions;

    public StudentController(DatabaseContext context,
        IOptions<DatabaseOptions> options,
        ILogger<StudentController> logger)
    {
        _context = context;
        _logger = logger;
        _databaseOptions = options.Value;
    }

    // GET
    [HttpGet(Name = "GetStudents")]
    public IEnumerable<Models.Student> Get()
    {
        _logger.LogInformation("{op}",_databaseOptions.CommandTimeout);
        return _context.Students.ToList();
    }
}