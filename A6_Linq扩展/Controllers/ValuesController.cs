using System.Linq.Expressions;
using A6_Linq扩展.Data;
using Microsoft.AspNetCore.Mvc;

namespace A6_Linq扩展.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ValuesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Product> Get(string name,
        string desc,
        string categoryName)
    {
        var products = _dbContext.Products.AsQueryable();
        // 1.if(...){...}
        // if (!string.IsNullOrWhiteSpace(name))
        //     products = products.Where(x => x.Name != null && (x.Name == name || x.Name.Contains(name)));

        // 2.如果name是空的，就不会执行x.Name == name 不为空就会执行x.Name == name
        // products = products.Where(x => string.IsNullOrWhiteSpace(name) || x.Name == name);

        // 3.linq扩展
        products.WhereIf(!string.IsNullOrWhiteSpace(name), p => p.Name == name)
            .WhereIf(!string.IsNullOrWhiteSpace(desc), p => p.Desc != null && p.Desc.Contains(desc));


        return products;
    }
}

/// <summary>
///     linq 扩展
/// </summary>
public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable,
        bool condition,
        Expression<Func<T, bool>> expression)
    {
        return condition ? queryable.Where(expression) : queryable;
    }
}