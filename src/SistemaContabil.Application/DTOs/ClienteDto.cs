using System.ComponentModel.DataAnnotations;
using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Application.DTOs;

public class ClienteDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; }
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
        Senha = cliente.Senha;
        Ativo = cliente.Ativo;
    }
}
public class CreateClienteRequest
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

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, ErrorMessage = "Senha não pode ter mais de 100 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [RegularExpression("^[SN]$", ErrorMessage = "Ativo deve ser 'S' (Ativo) ou 'N' (Inativo)")]
    public string Ativo { get; set; } = "S";
}

public class UpdateClienteRequest
{
    [Required(ErrorMessage = "Nome do cliente é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome não pode ter mais de 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Email não pode ter mais de 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    public string Senha { get; set; }

    [RegularExpression("^[SN]$", ErrorMessage = "Ativo deve ser 'S' (Ativo) ou 'N' (Inativo)")]
    public string Ativo { get; set; } = "S";
}

public class ClienteResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; }
    public char Ativo { get; set; }

    public ClienteResponse() { }

    public ClienteResponse(Cliente cliente)
    {
        Id = cliente.Id;
        Nome = cliente.Nome;
        DataCadastro = cliente.DataCadastro;
        Cpf = cliente.Cpf;
        Email = cliente.Email;
        Senha = cliente.Senha;
        Ativo = cliente.Ativo;
    }
}
