using System.ComponentModel;
using IdempotentAPI.MinimalAPI;
using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Application.DTOs;
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
            .Produces<List<PagamentoResponse>>();

        pagamentos.MapGet("/{id:int}", GetPagamento)
            .WithName("GetPagamentoById")
            .WithSummary("Busca um método de pagamento por id")
            .Produces<PagamentoResponse>(200)
            .Produces(404);

        pagamentos.MapPost("/", CreatePagamento)
            .WithName("CreatePagamento")
            .WithSummary("Cria um novo método de pagamento")
            .Produces<PagamentoResponse>(201)
            .Produces(400)
            .AddEndpointFilter<IdempotentAPIEndpointFilter>();

        #endregion
    }

    #region Services

    static async Task<IResult> GetAllPagamentos(SistemaContabilDb db)
    {
        return TypedResults.Ok(await db.Pagamentos
            .Select(p => new PagamentoResponse(p))
            .ToListAsync());
    }

    static async Task<IResult> GetPagamento(
        [Description("id do método de pagamento que será buscado.")] int id,
        SistemaContabilDb db)
    {
        return await db.Pagamentos.FindAsync(id) is Pagamento pagamento
            ? TypedResults.Ok(new PagamentoResponse(pagamento))
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
            Id = await db.Pagamentos.Select(p => p.Id).DefaultIfEmpty().MaxAsync() + 1,
            MetodoPagamento = pagamentoRequest.MetodoPagamento.Trim()
        };

        db.Pagamentos.Add(pagamento);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/pagamentos/{pagamento.Id}", pagamento);
    }
    #endregion
}

public record PagamentoRequest(string MetodoPagamento);
