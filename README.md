# net-demo
https://www.youtube.com/@MilanJovanovicTech/playlists

### 使用JSON配置数据库参数

### 优化EF的批量操作

### EF 仓储模式

### 拆分查询

拆分查询可以优化笛卡尔积爆炸问题

```C#
using (var context = new BloggingContext())
{
    var blogs = context.Blogs
        .Include(blog => blog.Posts)
        .AsSplitQuery()
        .ToList();
}
```



全局配置拆分查询

```C#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=EFQuerying;Trusted_Connection=True",
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
}
```







### 

