using System.ComponentModel;
using IdempotentAPI.MinimalAPI;
using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Api.Endpoints;

public static class ItemVendaEndpoints
{
    public static void RegisterItemVendaEndpoints(this WebApplication app)
    {
        #region Routes

        var itensVenda = app.MapGroup("/itens-venda").WithTags("itens-venda");

        itensVenda.MapGet("/", GetAllItensVenda)
            .WithName("GetAllItensVenda")
            .WithSummary("Apresenta todos os itens de venda")
            .Produces<List<ItemVendaResponse>>();

        itensVenda.MapGet("/{id:int}", GetItemVenda)
            .WithName("GetItemVendaById")
            .WithSummary("Busca um item de venda por id")
            .Produces<ItemVendaResponse>(200)
            .Produces(404);

        itensVenda.MapPost("/", CreateItemVenda)
            .WithName("CreateItemVenda")
            .WithSummary("Cria um novo item de venda")
            .Produces<ItemVendaResponse>(201)
            .Produces(400);

        itensVenda.MapPut("/{id:int}", UpdateItemVenda)
            .WithName("UpdateItemVendaById")
            .WithSummary("Atualiza um item de venda pelo seu id")
            .Produces(204)
            .Produces(404)
            .Produces(400);
        #endregion
    }

    #region Services

    static async Task<IResult> GetAllItensVenda(SistemaContabilDb db)
    {
        return TypedResults.Ok(await db.ItensVenda
            .Select(i => new ItemVendaResponse(i))
            .ToListAsync());
    }

    static async Task<IResult> GetItemVenda(
        [Description("id do item de venda que será buscado.")] int id,
        SistemaContabilDb db)
    {
        return await db.ItensVenda.FindAsync(id) is ItemVenda itemVenda
            ? TypedResults.Ok(new ItemVendaResponse(itemVenda))
            : TypedResults.NotFound();
    }

    static async Task<IResult> CreateItemVenda(
        [Description("Item de venda a ser cadastrado.")] CreateItemVendaRequest itemVendaRequest,
        SistemaContabilDb db)
    {
        if (!await db.Produtos.AnyAsync(p => p.Id == itemVendaRequest.ProdutoId))
        {
            return TypedResults.BadRequest(new { message = "Produto não encontrado" });
        }

        if (!await db.Vendas.AnyAsync(v => v.Id == itemVendaRequest.VendaId))
        {
            return TypedResults.BadRequest(new { message = "Venda não encontrada" });
        }

        var itemVenda = new ItemVenda
        {
            Id = await db.ItensVenda.Select(i => i.Id).DefaultIfEmpty().MaxAsync() + 1,
            Quantidade = itemVendaRequest.Quantidade,
            ValorUnitario = itemVendaRequest.ValorUnitario,
            ProdutoId = itemVendaRequest.ProdutoId,
            VendaId = itemVendaRequest.VendaId
        };

        db.ItensVenda.Add(itemVenda);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/itens-venda/{itemVenda.Id}", new ItemVendaResponse(itemVenda));
    }

    static async Task<IResult> UpdateItemVenda(
        [Description("id do item de venda que será atualizado.")] int id,
        UpdateItemVendaRequest itemVendaRequest,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();
        var itemVenda = await db.ItensVenda.FindAsync(id);

        if (itemVenda is null) return TypedResults.NotFound();

        itemVenda.Quantidade = itemVendaRequest.Quantidade;
        itemVenda.ValorUnitario = itemVendaRequest.ValorUnitario;
        itemVenda.ProdutoId = itemVendaRequest.ProdutoId;
        itemVenda.VendaId = itemVendaRequest.VendaId;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    #endregion
}

public record ItemVendaRequest(int Quantidade, int ProdutoId, int VendaId);
