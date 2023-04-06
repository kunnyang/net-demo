using Microsoft.EntityFrameworkCore;

namespace A4_使用拆分查询;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Contributor> Contributors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(b => b.HasData(new Blog
        {
            Id = 1,
            Name = "tom's blog"
        }));
        var posts = Enumerable.Range(1, 10)
            .Select(id => new Post
            {
                Id = id,
                Name = $"post # {id}",
                BlogId = 1
            });

        var contributors = Enumerable.Range(1, 10)
            .Select(id => new Contributor
            {
                Id = id,
                Name = $"Contributor # {id}",
                BlogId = 1
            });

        var comments = Enumerable.Range(1, 10)
            .Select(id => new Comment
            {
                Id = id,
                Name = $"Contributor # {id}",
                PostId = id
            });

        modelBuilder.Entity<Post>(p => p.HasData(posts));
        modelBuilder.Entity<Contributor>(p => p.HasData(contributors));
        modelBuilder.Entity<Comment>(c => c.HasData(comments));
    }
}

public class Blog
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Post>? Posts { get; set; }
    public List<Contributor>? Contributors { get; set; }
}

public class Post
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int BlogId { get; set; }

    public List<Comment>? Comments { get; set; }
}

public class Contributor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int BlogId { get; set; }
}

public class Comment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int PostId { get; set; }
}