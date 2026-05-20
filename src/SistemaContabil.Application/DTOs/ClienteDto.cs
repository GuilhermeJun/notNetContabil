using System.ComponentModel.DataAnnotations;
using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Application.DTOs;

[Serializable]
public class ClienteDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public char Ativo { get; set; }

    public ClienteDto()
    {
    }

    public ClienteDto(Cliente cliente)
    {
        Id = cliente.Id;
        Nome = cliente.Nome;
        DataCadastro = cliente.DataCadastro;
        Cpf = cliente.Cpf;
        Email = cliente.Email;
        Ativo = cliente.Ativo;
    }
}

public class CriarClienteDto
{
    [Required(ErrorMessage = "Nome do cliente é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome não pode ter mais de 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "CPF/CNPJ é obrigatório")]
    [StringLength(14, ErrorMessage = "CPF/CNPJ não pode ter mais de 14 caracteres")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Email não pode ter mais de 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    [RegularExpression("^[SN]$", ErrorMessage = "Ativo deve ser 'S' (Ativo) ou 'N' (Inativo)")]
    public string Ativo { get; set; } = "S";

    public char AtivoChar => !string.IsNullOrEmpty(Ativo) ? Ativo[0] : 'S';
}

public class AtualizarClienteDto
{
    [Required(ErrorMessage = "Nome do cliente é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome não pode ter mais de 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Email não pode ter mais de 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    [RegularExpression("^[SN]$", ErrorMessage = "Ativo deve ser 'S' (Ativo) ou 'N' (Inativo)")]
    public string Ativo { get; set; } = "S";
}
