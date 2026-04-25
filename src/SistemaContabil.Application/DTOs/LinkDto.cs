namespace SistemaContabil.Application.DTOs;

public class LinkDto
{
    public string Rel { get; set; } = string.Empty;

    public string Href { get; set; } = string.Empty;

    public string Method { get; set; } = "GET";

    public LinkDto(string rel, string href, string method = "GET")
    {
        Rel = rel;
        Href = href;
        Method = method;
    }
}
