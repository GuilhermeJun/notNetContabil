using System.ComponentModel;
using IdempotentAPI.MinimalAPI;
using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Api.Endpoints;

public static class ProdutoEndpoints
{
    public static void RegisterProdutoEndpoints(this WebApplication app)
    {
        #region Routes

        var produtos = app.MapGroup("/produtos").WithTags("produtos");

        produtos.MapGet("/", GetAllProdutos)
            .WithName("GetAllProdutos")
            .WithSummary("Apresenta todos os produtos")
            .Produces<List<Produto>>();

        produtos.MapGet("/{id:int}", GetProduto)
            .WithName("GetProdutoById")
            .WithSummary("Busca um produto por id")
            .Produces<Produto>(200)
            .Produces(404);

        produtos.MapPost("/", CreateProduto)
            .WithName("CreateProduto")
            .WithSummary("Cria um novo produto")
            .Produces<Produto>(201)
            .Produces(400)
            .AddEndpointFilter<IdempotentAPIEndpointFilter>();

        produtos.MapPut("/{id:int}", UpdateProduto)
            .WithName("UpdateProdutoById")
            .WithSummary("Atualiza um produto pelo seu id")
            .Produces(204)
            .Produces(404)
            .Produces(400);

        produtos.MapDelete("/{id:int}", DeleteProduto)
            .WithName("DeleteProdutoById")
            .WithSummary("Deleta um produto pelo seu id")
            .Produces(204)
            .Produces(404)
            .Produces(400);

        #endregion
    }

    #region Services

    static async Task<IResult> GetAllProdutos(SistemaContabilDb db)
    {
        return TypedResults.Ok(await db.Produtos.ToListAsync());
    }

    static async Task<IResult> GetProduto(
        [Description("id do produto que será buscado.")] int id,
        SistemaContabilDb db)
    {
        return await db.Produtos.FindAsync(id) is Produto produto
            ? TypedResults.Ok(produto)
            : TypedResults.NotFound();
    }

    static async Task<IResult> CreateProduto(
        [Description("Produto a ser cadastrado.")] ProdutoRequest produtoRequest,
        SistemaContabilDb db)
    {
        if (string.IsNullOrWhiteSpace(produtoRequest.Nome))
        {
            return TypedResults.BadRequest(new { message = "Nome do produto é obrigatório" });
        }

        var produto = new Produto
        {
            Id = await db.Produtos.Select(p => p.Id).DefaultIfEmpty().MaxAsync() + 1,
            Nome = produtoRequest.Nome.Trim(),
            Descricao = produtoRequest.Descricao.Trim(),
            valor = produtoRequest.Valor,
            estoque = produtoRequest.Estoque
        };

        db.Produtos.Add(produto);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/produtos/{produto.Id}", produto);
    }

    static async Task<IResult> UpdateProduto(
        [Description("id do produto que será atualizado.")] int id,
        ProdutoRequest produtoRequest,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();
        var produto = await db.Produtos.FindAsync(id);

        if (produto is null) return TypedResults.NotFound();

        produto.Nome = produtoRequest.Nome.Trim();
        produto.Descricao = produtoRequest.Descricao.Trim();
        produto.valor = produtoRequest.Valor;
        produto.estoque = produtoRequest.Estoque;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    static async Task<IResult> DeleteProduto(
        [Description("id do produto que será deletado.")] int id,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();

        if (await db.ItensVenda.AnyAsync(i => i.ProdutoId == id))
        {
            return TypedResults.BadRequest(new { message = "Produto possui itens de venda vinculados" });
        }

        if (await db.Produtos.FindAsync(id) is Produto produto)
        {
            db.Produtos.Remove(produto);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    #endregion
}

public record ProdutoRequest(string Nome, string Descricao, float Valor, int Estoque);
