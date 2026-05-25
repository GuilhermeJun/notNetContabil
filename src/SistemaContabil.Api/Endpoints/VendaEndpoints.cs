using System.ComponentModel;
using IdempotentAPI.MinimalAPI;
using Microsoft.EntityFrameworkCore;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Api.Endpoints;

public static class VendaEndpoints
{
    public static void RegisterVendaEndpoints(this WebApplication app)
    {
        #region Routes

        var vendas = app.MapGroup("/vendas").WithTags("vendas");

        vendas.MapGet("/", GetAllVendas)
            .WithName("GetAllVendas")
            .WithSummary("Apresenta todas as vendas")
            .Produces<List<VendaRequest>>();

        vendas.MapGet("/{id:int}", GetVenda)
            .WithName("GetVendaById")
            .WithSummary("Busca uma venda por id")
            .Produces<VendaRequest>(200)
            .Produces(404);

        vendas.MapPost("/", CreateVenda)
            .WithName("CreateVenda")
            .WithSummary("Cria uma nova venda")
            .Produces<VendaRequest>(201)
            .Produces(400);
            //.AddEndpointFilter<IdempotentAPIEndpointFilter>();
        #endregion
    }

    #region Services

    static async Task<IResult> GetAllVendas(SistemaContabilDb db)
    {
        return TypedResults.Ok(await db.Vendas
            .Include(v => v.Cliente)
            .Include(v => v.RegistroContabil)
            .Select(v => ToDto(v))
            .ToListAsync());
    }

    static async Task<IResult> GetVenda(
        [Description("id da venda que será buscada.")] int id,
        SistemaContabilDb db)
    {
        var venda = await db.Vendas
            .Include(v => v.Cliente)
            .Include(v => v.RegistroContabil)
            .FirstOrDefaultAsync(v => v.Id == id);

        return venda is null ? TypedResults.NotFound() : TypedResults.Ok(ToDto(venda));
    }

    static async Task<IResult> CreateVenda(
        [Description("Venda a ser cadastrada.")] CreateVendaRequest vendaDto,
        SistemaContabilDb db)
    {
        var validation = await ValidateReferencesAsync(vendaDto.ClienteId, vendaDto.RegContId, vendaDto.PagamentoId, db);
        if (validation is not null) return validation;

        var venda = new Venda
        {
            Id = await GetNextVendaIdAsync(db),
            ClienteId = vendaDto.ClienteId,
            RegContId = vendaDto.RegContId,
            PagamentoId = vendaDto.PagamentoId,
            MarketplaceId = vendaDto.MarketplaceId,
            DataVenda = DateTime.Now,
            Status = string.IsNullOrWhiteSpace(vendaDto.Status) ? "Criada" : vendaDto.Status.Trim()
        };

        db.Vendas.Add(venda);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/vendas/{venda.Id}", ToDto(venda));
    }

    static async Task<IResult?> ValidateReferencesAsync(int clienteId, int regContId, int pagamentoId, SistemaContabilDb db)
    {
        if (!await db.Clientes.AnyAsync(c => c.Id == clienteId))
        {
            return TypedResults.BadRequest(new { message = "Cliente não encontrado" });
        }

        if (!await db.RegistrosContabeis.AnyAsync(r => r.Id == regContId))
        {
            return TypedResults.BadRequest(new { message = "Registro contábil não encontrado" });
        }

        if (pagamentoId != 0)
        {
            if (!await db.Pagamentos.AnyAsync(p => p.Id == pagamentoId))
            {
                return TypedResults.BadRequest(new { message = "Método de pagamento não encontrado" });
            }
        }

        return null;
    }

    static VendaRequest ToDto(Venda venda)
    {
        return new VendaRequest
        {
            Id = venda.Id,
            ClienteId = venda.ClienteId,
            NomeCliente = venda.Cliente?.Nome ?? string.Empty,
            RegContId = venda.RegContId,
            Valor = venda.RegistroContabil?.Valor ?? 0,
        };
    }

    static async Task<int> GetNextVendaIdAsync(SistemaContabilDb db)
    {
        var sequenceValue = await EndpointSequenceHelper.GetNextValueAsync(db, "vendas_seq", "seq_vendas");
        return sequenceValue > 0 ? sequenceValue : await db.Vendas.Select(v => v.Id).DefaultIfEmpty().MaxAsync() + 1;
    }

    #endregion
}
