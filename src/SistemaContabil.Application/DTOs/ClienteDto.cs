using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using SistemaContabil.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SistemaContabil.Application.DTOs;

[Serializable]
public class ClienteDto
{
        /*
        : IEndpointParameterMetadataProvider, IBindableFromHttpContext<ClienteDto>
        {
            public static void PopulateMetadata(
                ParameterInfo parameter,
                EndpointBuilder builder)
    {
        builder.Metadata.Add(
            new AcceptsMetadata(
                new[] { "application/json", "text/json", "application/xml", "text/xml" },
                typeof(ClienteDto)
            )
        );
    }
    

    public static async ValueTask<ClienteDto?> BindAsync(
        HttpContext context,
        ParameterInfo parameter)
    {
        var contentType = context.Request.ContentType?.Split(';')[0].Trim().ToLowerInvariant();

        // Handle JSON
        if (contentType is "application/json" or "text/json" or null)
        {
            try
            {
                var todo = await JsonSerializer.DeserializeAsync<Cliente>(context.Request.Body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }, context.RequestAborted);

                if (todo is null) return null;
                return new ClienteDto(todo);
            }
            catch
            {
                return null;
            }
        }

        // Handle XML
        if (contentType is "application/xml" or "text/xml")
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Cliente));
                if (serializer.Deserialize(context.Request.Body) is Cliente todo)
                {
                    return new ClienteDto(todo);
                }
            }
            catch
            {
                return null;
            }
        }

        return null;
    }
    */

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

    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(11, ErrorMessage = "CPF não pode ter mais de 11 caracteres")]
    public string Cpf { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Email não pode ter mais de 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, ErrorMessage = "Senha não pode ter mais de 100 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [RegularExpression("^[SN]$", ErrorMessage = "Ativo deve ser 'S' (Ativo) ou 'N' (Inativo)")]
    public char Ativo { get; set; } = 'S';
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

    [RegularExpression("^[SN]$", ErrorMessage = "Ativo deve ser 'S' (Ativo) ou 'N' (Inativo)")]
    public char Ativo { get; set; }
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

public class ClienteHateoasResponse : ClienteResponse
{
    [JsonPropertyName("_links")]
    public List<HateoasLinkDto> Links { get; set; } = [];

    public ClienteHateoasResponse() { }

    public ClienteHateoasResponse(Cliente cliente) : base(cliente)
    {
    }
}

public record HateoasLinkDto(string Rel, string Href, string Method);
