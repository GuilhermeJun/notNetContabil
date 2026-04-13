using System.ComponentModel.DataAnnotations;

namespace SistemaContabil.Application.DTOs;

/// <summary>
/// DTO para Conta
/// </summary>
public class ContaDto
{
    /// <summary>
    /// Identificador único da conta contábil
    /// </summary>
    public int IdContaContabil { get; set; }

    /// <summary>
    /// Nome da conta contábil
    /// </summary>
    [Required(ErrorMessage = "Nome da conta contábil é obrigatório")]
    [StringLength(70, ErrorMessage = "Nome não pode ter mais de 70 caracteres")]
    public string NomeContaContabil { get; set; } = string.Empty;

    /// <summary>
    /// Tipo da conta (R = Receita, D = Despesa)
    /// </summary>
    [Required(ErrorMessage = "Tipo da conta é obrigatório")]
    [RegularExpression("^[RD]$", ErrorMessage = "Tipo deve ser 'R' (Receita) ou 'D' (Despesa)")]
    public char Tipo { get; set; }

    /// <summary>
    /// ID do cliente associado (opcional)
    /// </summary>
    public int? ClienteId { get; set; }

    /// <summary>
    /// Descrição do tipo da conta
    /// </summary>
    public string TipoDescricao { get; set; } = string.Empty;

    /// <summary>
    /// Quantidade de registros contábeis associados
    /// </summary>
    public int QuantidadeRegistros { get; set; }

    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTime? DataCriacao { get; set; }
}

/// <summary>
/// DTO para criação de Conta
/// </summary>
public class CriarContaDto
{
    /// <summary>
    /// Nome da conta contábil
    /// </summary>
    [Required(ErrorMessage = "Nome da conta contábil é obrigatório")]
    [StringLength(70, ErrorMessage = "Nome não pode ter mais de 70 caracteres")]
    public string NomeContaContabil { get; set; } = string.Empty;

    /// <summary>
    /// Tipo da conta (R = Receita, D = Despesa)
    /// </summary>
    [Required(ErrorMessage = "Tipo da conta é obrigatório")]
    [RegularExpression("^[RD]$", ErrorMessage = "Tipo deve ser 'R' (Receita) ou 'D' (Despesa)")]
    public char Tipo { get; set; }

    /// <summary>
    /// ID do cliente associado (opcional)
    /// </summary>
    public int? ClienteId { get; set; }
}

/// <summary>
/// DTO para atualização de Conta
/// </summary>
public class AtualizarContaDto
{
    /// <summary>
    /// Nome da conta contábil
    /// </summary>
    [Required(ErrorMessage = "Nome da conta contábil é obrigatório")]
    [StringLength(70, ErrorMessage = "Nome não pode ter mais de 70 caracteres")]
    public string NomeContaContabil { get; set; } = string.Empty;

    /// <summary>
    /// Tipo da conta (R = Receita, D = Despesa)
    /// </summary>
    [Required(ErrorMessage = "Tipo da conta é obrigatório")]
    [RegularExpression("^[RD]$", ErrorMessage = "Tipo deve ser 'R' (Receita) ou 'D' (Despesa)")]
    public char Tipo { get; set; }

    /// <summary>
    /// ID do cliente associado (opcional)
    /// </summary>
    public int? ClienteId { get; set; }
}

/// <summary>
/// DTO para resposta de Conta com detalhes
/// </summary>
public class ContaDetalhesDto
{
    /// <summary>
    /// Identificador único da conta contábil
    /// </summary>
    public int IdContaContabil { get; set; }

    /// <summary>
    /// Nome da conta contábil
    /// </summary>
    public string NomeContaContabil { get; set; } = string.Empty;

    /// <summary>
    /// Tipo da conta (R = Receita, D = Despesa)
    /// </summary>
    public char Tipo { get; set; }

    /// <summary>
    /// ID do cliente associado (opcional)
    /// </summary>
    public int? ClienteId { get; set; }

    /// <summary>
    /// Descrição do tipo da conta
    /// </summary>
    public string TipoDescricao { get; set; } = string.Empty;

    /// <summary>
    /// Lista de registros contábeis associados
    /// </summary>
    public List<RegistroContabilDto> RegistrosContabeis { get; set; } = new();

    /// <summary>
    /// Total de registros
    /// </summary>
    public int TotalRegistros { get; set; }

    /// <summary>
    /// Valor total dos registros
    /// </summary>
    public decimal ValorTotal { get; set; }
}
