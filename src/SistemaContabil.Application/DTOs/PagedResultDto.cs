namespace SistemaContabil.Application.DTOs;

public class PagedResultDto<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < TotalPages;

    public Dictionary<string, string> Links { get; set; } = new();
}
