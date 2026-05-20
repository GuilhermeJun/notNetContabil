using System.ComponentModel;
using IdempotentAPI.MinimalAPI;
using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Api.Endpoints;

public static class PagamentoEndpoints
{
    public static void RegisterPagamentoEndpoints(this WebApplication app)
    {
        #region Routes

        var pagamentos = app.MapGroup("/pagamentos").WithTags("pagamentos");

        pagamentos.MapGet("/", GetAllPagamentos)
            .WithName("GetAllPagamentos")
            .WithSummary("Apresenta todos os métodos de pagamento")
            .Produces<List<Pagamento>>();

        pagamentos.MapGet("/{id:int}", GetPagamento)
            .WithName("GetPagamentoById")
            .WithSummary("Busca um método de pagamento por id")
            .Produces<Pagamento>(200)
            .Produces(404);

        pagamentos.MapPost("/", CreatePagamento)
            .WithName("CreatePagamento")
            .WithSummary("Cria um novo método de pagamento")
            .Produces<Pagamento>(201)
            .Produces(400)
            .AddEndpointFilter<IdempotentAPIEndpointFilter>();

        pagamentos.MapPut("/{id:int}", UpdatePagamento)
            .WithName("UpdatePagamentoById")
            .WithSummary("Atualiza um método de pagamento pelo seu id")
            .Produces(204)
            .Produces(404)
            .Produces(400);

        pagamentos.MapDelete("/{id:int}", DeletePagamento)
            .WithName("DeletePagamentoById")
            .WithSummary("Deleta um método de pagamento pelo seu id")
            .Produces(204)
            .Produces(404)
            .Produces(400);

        #endregion
    }

    #region Services

    static async Task<IResult> GetAllPagamentos(SistemaContabilDb db)
    {
        return TypedResults.Ok(await db.Pagamentos.ToListAsync());
    }

    static async Task<IResult> GetPagamento(
        [Description("id do método de pagamento que será buscado.")] int id,
        SistemaContabilDb db)
    {
        return await db.Pagamentos.FindAsync(id) is Pagamento pagamento
            ? TypedResults.Ok(pagamento)
            : TypedResults.NotFound();
    }

    static async Task<IResult> CreatePagamento(
        [Description("Método de pagamento a ser cadastrado.")] PagamentoRequest pagamentoRequest,
        SistemaContabilDb db)
    {
        if (string.IsNullOrWhiteSpace(pagamentoRequest.MetodoPagamento))
        {
            return TypedResults.BadRequest(new { message = "Método de pagamento é obrigatório" });
        }

        var pagamento = new Pagamento
        {
            IdPagamento = await db.Pagamentos.Select(p => p.IdPagamento).DefaultIfEmpty().MaxAsync() + 1,
            MetodoPagamento = pagamentoRequest.MetodoPagamento.Trim()
        };

        db.Pagamentos.Add(pagamento);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/pagamentos/{pagamento.IdPagamento}", pagamento);
    }

    static async Task<IResult> UpdatePagamento(
        [Description("id do método de pagamento que será atualizado.")] int id,
        PagamentoRequest pagamentoRequest,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();
        var pagamento = await db.Pagamentos.FindAsync(id);

        if (pagamento is null) return TypedResults.NotFound();

        pagamento.MetodoPagamento = pagamentoRequest.MetodoPagamento.Trim();
        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    static async Task<IResult> DeletePagamento(
        [Description("id do método de pagamento que será deletado.")] int id,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();

        if (await db.Vendas.AnyAsync(v => v.PagamentoId == id))
        {
            return TypedResults.BadRequest(new { message = "Método de pagamento possui vendas vinculadas" });
        }

        if (await db.Pagamentos.FindAsync(id) is Pagamento pagamento)
        {
            db.Pagamentos.Remove(pagamento);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    #endregion
}

public record PagamentoRequest(string MetodoPagamento);
