namespace SistemaContabil.Application.DTOs;

public class FiltroContaDto : SearchRequestDto
{
    public string? Nome { get; set; }

    public char? Tipo { get; set; }

    public int? ClienteId { get; set; }
}
