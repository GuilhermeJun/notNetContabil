using System.ComponentModel.DataAnnotations;
using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Application.DTOs;

public class ItemVendaResponse
{
    public int Id { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public int ProdutoId { get; set; }
    public int VendaId { get; set; }

    public ItemVendaResponse() { }
    public ItemVendaResponse(ItemVenda item)
    {
        Id = item.Id;
        Quantidade = item.Quantidade;
        ValorUnitario = item.ValorUnitario;
        ProdutoId = item.ProdutoId;
        VendaId = item.VendaId;
    }
}

public class CreateItemVendaRequest
{
    [Required]
    public int Quantidade { get; set; }

    [Required]
    public decimal ValorUnitario { get; set; }

    [Required]
    public int ProdutoId { get; set; }

    [Required]
    public int VendaId { get; set; }
}

public class UpdateItemVendaRequest
{
    [Required]
    public int Quantidade { get; set; }

    [Required]
    public decimal ValorUnitario { get; set; }

    [Required]
    public int ProdutoId { get; set; }

    [Required]
    public int VendaId { get; set; }
}
