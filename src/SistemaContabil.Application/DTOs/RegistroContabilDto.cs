using System.ComponentModel.DataAnnotations;

namespace SistemaContabil.Application.DTOs;

public class RegistroContabilDto
{
    public int IdRegCont { get; set; }

    public decimal Valor { get; set; }

    public int ContaId { get; set; }

    public int? CentroCustoId { get; set; }

    public string NomeConta { get; set; } = string.Empty;

    public string NomeCentroCusto { get; set; } = string.Empty;

    public DateTime DataCriacao { get; set; }

    public DateTime? DataAtualizacao { get; set; }
}

public class CriarRegistroContabilDto
{
    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(0.01, 999999.99, ErrorMessage = "Valor deve estar entre 0,01 e 999.999,99")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "ID da conta é obrigatório")]
    public int ContaId { get; set; }

    public int? CentroCustoId { get; set; }
}

public class AtualizarRegistroContabilDto
{
    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(0.01, 999999.99, ErrorMessage = "Valor deve estar entre 0,01 e 999.999,99")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "ID da conta é obrigatório")]
    public int ContaId { get; set; }

    public int? CentroCustoId { get; set; }
}
