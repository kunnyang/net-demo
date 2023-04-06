namespace A5_规范设计模式;

public interface ISpecification<in T>
{
    bool IsSatisfied(T obj);
}

internal class In21CenturySpecification : ISpecification<DateTime>
{
    private readonly DateTime _end = new(2101, 01, 01);
    private readonly DateTime _start = new(2000, 01, 01);

    public bool IsSatisfied(DateTime obj)
    {
        return obj >= _start && obj < _end;
    }
}