namespace SistemaContabil.Application.DTOs;

public class SearchRequestDto
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? SortBy { get; set; }

    public string SortOrder { get; set; } = "asc";

    public void Validate()
    {
        if (Page < 1)
            Page = 1;

        if (PageSize < 1)
            PageSize = 10;

        if (PageSize > 100)
            PageSize = 100;

        if (string.IsNullOrWhiteSpace(SortOrder))
            SortOrder = "asc";

        SortOrder = SortOrder.ToLowerInvariant();
        if (SortOrder != "asc" && SortOrder != "desc")
            SortOrder = "asc";
    }

    public bool IsDescending => SortOrder?.ToLowerInvariant() == "desc";
}
