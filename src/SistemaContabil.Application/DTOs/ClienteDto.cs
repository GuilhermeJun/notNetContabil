using System.ComponentModel.DataAnnotations;

namespace SistemaContabil.Application.DTOs;

public class ClienteDto
{
    public int IdCliente { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
    public string CpfCnpj { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Ativo { get; set; } = "S";
    public string StatusDescricao => Ativo == "S" ? "Ativo" : "Inativo";
}

public class CriarClienteDto
{
    [Required(ErrorMessage = "Nome do cliente é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome não pode ter mais de 100 caracteres")]
    public string NomeCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "CPF/CNPJ é obrigatório")]
    [StringLength(14, ErrorMessage = "CPF/CNPJ não pode ter mais de 14 caracteres")]
    public string CpfCnpj { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Email não pode ter mais de 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, ErrorMessage = "Senha não pode ter mais de 100 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [RegularExpression("^[SN]$", ErrorMessage = "Ativo deve ser 'S' (Ativo) ou 'N' (Inativo)")]
    public string Ativo { get; set; } = "S";

    public char AtivoChar => !string.IsNullOrEmpty(Ativo) ? Ativo[0] : 'S';
}

public class AtualizarClienteDto
{
    [Required(ErrorMessage = "Nome do cliente é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome não pode ter mais de 100 caracteres")]
    public string NomeCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Email não pode ter mais de 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    [RegularExpression("^[SN]$", ErrorMessage = "Ativo deve ser 'S' (Ativo) ou 'N' (Inativo)")]
    public string Ativo { get; set; } = "S";
}
