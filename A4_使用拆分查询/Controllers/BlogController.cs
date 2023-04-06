using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace A4_使用拆分查询.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly AppDbContext _context;

    public BlogController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetBlogs")]
    public IEnumerable<Blog> Get()
    {
        var blogs =
            _context.Blogs
                .Include(b => b.Posts)
                .Include(b => b.Contributors);
        // .AsSplitQuery();

        /*
         *不使用拆分查询 a*b*c 笛卡尔积爆炸
         * SELECT [b].[Id], [b].[Name], [p].[Id], [p].[BlogId], [p].[Name], [c].[Id], [c].[BlogId], [c].[Name]
              FROM [Blogs] AS [b]
              LEFT JOIN [Posts] AS [p] ON [b].[Id] = [p].[BlogId]
              LEFT JOIN [Contributors] AS [c] ON [b].[Id] = [c].[BlogId]
              ORDER BY [b].[Id], [p].[Id]
         */

        /*
        AsSplitQuery
        相当于分别产生三条sql blogs（结果1条）、posts（结果10）、Contributors（结果10）
         * SELECT [b].[Id], [b].[Name]
FROM [Blogs] AS [b]
ORDER BY [b].[Id]

SELECT [p].[Id], [p].[BlogId], [p].[Name], [b].[Id]
FROM [Blogs] AS [b]
        INNER JOIN [Posts] AS [p] ON [b].[Id] = [p].[BlogId]
ORDER BY [b].[Id]

SELECT [c].[Id], [c].[BlogId], [c].[Name], [b].[Id]
FROM [Blogs] AS [b]
        INNER JOIN [Contributors] AS [c] ON [b].[Id] = [c].[BlogId]
ORDER BY [b].[Id]
         */
        return blogs;
    }

    [HttpGet(Name = "GetBlogsComments")]
    public IEnumerable<Blog> GetBlogsComments()
    {
        var blogs =
            _context.Blogs
                .Include(b => b.Posts)!
                .ThenInclude(p => p.Comments);

        return blogs;
    }
}