using System.ComponentModel.DataAnnotations;
using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Application.DTOs;

public class MarketplaceResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal ValorComissao { get; set; }

    public MarketplaceResponse() { }
    public MarketplaceResponse(Marketplace m)
    {
        Id = m.Id;
        Nome = m.Nome;
        ValorComissao = m.ValorComissao;
    }
}

public class CreateMarketplaceRequest
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public decimal ValorComissao { get; set; }
}

public class UpdateMarketplaceRequest
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public decimal ValorComissao { get; set; }
}
