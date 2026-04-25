namespace SistemaContabil.Application.DTOs;

public class FiltroClienteDto : SearchRequestDto
{
    public string? Nome { get; set; }

    public string? CpfCnpj { get; set; }

    public char? Ativo { get; set; }

    public string? Email { get; set; }
}
