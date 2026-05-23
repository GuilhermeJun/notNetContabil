using System.ComponentModel.DataAnnotations;
using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Application.DTOs;

public class ProdutoResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }

    public ProdutoResponse() { }

    public ProdutoResponse(Produto produto)
    {
        Id = produto.Id;
        Nome = produto.Nome;
        Descricao = produto.Descricao;
        Preco = produto.Preco;
        Estoque = produto.Estoque;
    }
}

public class CreateProdutoRequest
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(250)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [Range(0, 9999999.99)]
    public decimal Preco { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Estoque { get; set; }
}

public class UpdateProdutoRequest
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(250)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [Range(0, 9999999.99)]
    public decimal Preco { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Estoque { get; set; }
}
