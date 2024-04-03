namespace Entities.RequestFeatures;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; set; }

    public PagedList(List<T> items, int count, int pageNumber, int pagerSize)
    {
        MetaData = new MetaData
        {
            TotalCount = count,
            PageSize = pagerSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pagerSize)
        };
        AddRange(items);
    }

    public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToList();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}