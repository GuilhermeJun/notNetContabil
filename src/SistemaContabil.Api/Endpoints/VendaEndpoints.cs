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
            .Produces<List<VendasDto>>();

        vendas.MapGet("/{id:int}", GetVenda)
            .WithName("GetVendaById")
            .WithSummary("Busca uma venda por id")
            .Produces<VendasDto>(200)
            .Produces(404);

        vendas.MapPost("/", CreateVenda)
            .WithName("CreateVenda")
            .WithSummary("Cria uma nova venda")
            .Produces<VendasDto>(201)
            .Produces(400)
            .AddEndpointFilter<IdempotentAPIEndpointFilter>();

        vendas.MapPut("/{id:int}", UpdateVenda)
            .WithName("UpdateVendaById")
            .WithSummary("Atualiza uma venda pelo seu id")
            .Produces(204)
            .Produces(404)
            .Produces(400);

        vendas.MapDelete("/{id:int}", DeleteVenda)
            .WithName("DeleteVendaById")
            .WithSummary("Deleta uma venda pelo seu id")
            .Produces(204)
            .Produces(404)
            .Produces(400);

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
            .FirstOrDefaultAsync(v => v.IdVendas == id);

        return venda is null ? TypedResults.NotFound() : TypedResults.Ok(ToDto(venda));
    }

    static async Task<IResult> CreateVenda(
        [Description("Venda a ser cadastrada.")] VendaRequest vendaDto,
        SistemaContabilDb db)
    {
        var validation = await ValidateReferencesAsync(vendaDto.ClienteId, vendaDto.RegContId, vendaDto.PagamentoId, db);
        if (validation is not null) return validation;

        var venda = new Venda
        {
            IdVendas = await GetNextVendaIdAsync(db),
            ClienteId = vendaDto.ClienteId,
            RegContId = vendaDto.RegContId,
            PagamentoId = vendaDto.PagamentoId,
            MarketplaceId = vendaDto.MarketplaceId,
            VendaEventoId = vendaDto.VendaEventoId,
            DataVenda = DateTime.Now,
            Status = string.IsNullOrWhiteSpace(vendaDto.Status) ? "Criada" : vendaDto.Status.Trim()
        };

        db.Vendas.Add(venda);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/vendas/{venda.IdVendas}", ToDto(venda));
    }

    static async Task<IResult> UpdateVenda(
        [Description("id da venda que será atualizada.")] int id,
        VendaRequest vendaDto,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();
        var venda = await db.Vendas.FindAsync(id);

        if (venda is null) return TypedResults.NotFound();

        var validation = await ValidateReferencesAsync(vendaDto.ClienteId, vendaDto.RegContId, vendaDto.PagamentoId, db);
        if (validation is not null) return validation;

        venda.ClienteId = vendaDto.ClienteId;
        venda.RegContId = vendaDto.RegContId;
        venda.PagamentoId = vendaDto.PagamentoId;
        venda.MarketplaceId = vendaDto.MarketplaceId;
        venda.VendaEventoId = vendaDto.VendaEventoId;
        venda.Status = string.IsNullOrWhiteSpace(vendaDto.Status) ? venda.Status : vendaDto.Status.Trim();

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    static async Task<IResult> DeleteVenda(
        [Description("id da venda que será deletada.")] int id,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();

        if (await db.ItensVenda.AnyAsync(i => i.VendaId == id))
        {
            return TypedResults.BadRequest(new { message = "Venda possui itens vinculados" });
        }

        if (await db.Vendas.FindAsync(id) is Venda venda)
        {
            db.Vendas.Remove(venda);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    static async Task<IResult?> ValidateReferencesAsync(int clienteId, int regContId, int pagamentoId, SistemaContabilDb db)
    {
        if (!await db.Clientes.AnyAsync(c => c.Id == clienteId))
        {
            return TypedResults.BadRequest(new { message = "Cliente não encontrado" });
        }

        if (!await db.RegistrosContabeis.AnyAsync(r => r.IdRegCont == regContId))
        {
            return TypedResults.BadRequest(new { message = "Registro contábil não encontrado" });
        }

        if (!await db.Pagamentos.AnyAsync(p => p.IdPagamento == pagamentoId))
        {
            return TypedResults.BadRequest(new { message = "Método de pagamento não encontrado" });
        }

        return null;
    }

    static VendasDto ToDto(Venda venda)
    {
        return new VendasDto
        {
            IdVendas = venda.IdVendas,
            ClienteId = venda.ClienteId,
            NomeCliente = venda.Cliente?.Nome ?? string.Empty,
            RegContId = venda.RegContId,
            Valor = venda.RegistroContabil?.Valor ?? 0,
            VendaEventoId = venda.VendaEventoId
        };
    }

    static async Task<int> GetNextVendaIdAsync(SistemaContabilDb db)
    {
        var sequenceValue = await EndpointSequenceHelper.GetNextValueAsync(db, "vendas_seq", "seq_vendas");
        return sequenceValue > 0 ? sequenceValue : await db.Vendas.Select(v => v.IdVendas).DefaultIfEmpty().MaxAsync() + 1;
    }

    #endregion
}

public record VendaRequest(
    int ClienteId,
    int RegContId,
    int PagamentoId,
    int MarketplaceId,
    long? VendaEventoId,
    string? Status);
