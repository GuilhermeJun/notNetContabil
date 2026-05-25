using System.ComponentModel.DataAnnotations;

namespace SistemaContabil.Application.DTOs;

public class ContaRequest
{
    public int IdConta { get; set; }

    public string NomeConta { get; set; } = string.Empty;

    public char Tipo { get; set; }

    public int? ClienteId { get; set; }

    public string TipoDescricao { get; set; } = string.Empty;

    public int QuantidadeRegistros { get; set; }

    public DateTime? DataCriacao { get; set; }
}

public class CreateContaRequest
{
    [Required(ErrorMessage = "Nome da conta contábil é obrigatório")]
    [StringLength(70, ErrorMessage = "Nome não pode ter mais de 70 caracteres")]
    public string NomeConta { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo da conta é obrigatório")]
    [RegularExpression("^[RD]$", ErrorMessage = "Tipo deve ser 'R' (Receita) ou 'D' (Despesa)")]
    public char Tipo { get; set; }

    public int? ClienteId { get; set; }
}

public class UpdateContaRequest
{
    [Required(ErrorMessage = "Nome da conta contábil é obrigatório")]
    [StringLength(70, ErrorMessage = "Nome não pode ter mais de 70 caracteres")]
    public string NomeConta { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo da conta é obrigatório")]
    [RegularExpression("^[RD]$", ErrorMessage = "Tipo deve ser 'R' (Receita) ou 'D' (Despesa)")]
    public char Tipo { get; set; }

    public int? ClienteId { get; set; }
}
