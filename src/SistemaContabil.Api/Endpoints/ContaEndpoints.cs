using System.ComponentModel;
using IdempotentAPI.MinimalAPI;
using Microsoft.EntityFrameworkCore;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Api.Endpoints;

public static class ContaEndpoints
{
    public static void RegisterContaEndpoints(this WebApplication app)
    {
        #region Routes

        var contas = app.MapGroup("/contas").WithTags("contas");

        contas.MapGet("/", GetAllContas)
            .WithName("GetAllContas")
            .WithSummary("Apresenta todas as contas contábeis")
            .Produces<List<ContaRequest>>();

        contas.MapGet("/receitas", GetContasReceita)
            .WithName("GetContasReceita")
            .WithSummary("Apresenta todas as contas de receita")
            .Produces<List<ContaRequest>>();

        contas.MapGet("/despesas", GetContasDespesa)
            .WithName("GetContasDespesa")
            .WithSummary("Apresenta todas as contas de despesa")
            .Produces<List<ContaRequest>>();

        contas.MapGet("/{id:int}", GetConta)
            .WithName("GetContaById")
            .WithSummary("Busca uma conta por id")
            .Produces<ContaRequest>(200)
            .Produces(404);

        contas.MapPost("/", CreateConta)
            .WithName("CreateConta")
            .WithSummary("Cria uma nova conta")
            .Produces<ContaRequest>(201)
            .Produces(400)
            .AddEndpointFilter<IdempotentAPIEndpointFilter>();

        contas.MapPut("/{id:int}", UpdateConta)
            .WithName("UpdateContaById")
            .WithSummary("Atualiza uma conta pelo seu id")
            .Produces(204)
            .Produces(404)
            .Produces(400);

        contas.MapDelete("/{id:int}", DeleteConta)
            .WithName("DeleteContaById")
            .WithSummary("Deleta uma conta pelo seu id")
            .Produces(204)
            .Produces(404)
            .Produces(400);

        #endregion
    }

    #region Services

    static async Task<IResult> GetAllContas(SistemaContabilDb db)
    {
        return TypedResults.Ok(await db.Contas
            .Include(c => c.RegistrosContabeis)
            .Select(c => ToDto(c))
            .ToListAsync());
    }

    static async Task<IResult> GetContasReceita(SistemaContabilDb db)
    {
        return TypedResults.Ok(await db.Contas
            .Where(c => c.Tipo == 'R')
            .Include(c => c.RegistrosContabeis)
            .Select(c => ToDto(c))
            .ToListAsync());
    }

    static async Task<IResult> GetContasDespesa(SistemaContabilDb db)
    {
        return TypedResults.Ok(await db.Contas
            .Where(c => c.Tipo == 'D')
            .Include(c => c.RegistrosContabeis)
            .Select(c => ToDto(c))
            .ToListAsync());
    }

    static async Task<IResult> GetConta(
        [Description("id da conta que será buscada.")] int id,
        SistemaContabilDb db)
    {
        var conta = await db.Contas
            .Include(c => c.RegistrosContabeis)
            .FirstOrDefaultAsync(c => c.IdContaContabil == id);

        return conta is null ? TypedResults.NotFound() : TypedResults.Ok(ToDto(conta));
    }

    static async Task<IResult> CreateConta(
        [Description("Conta a ser cadastrada.")] CreateContaRequest contaDto,
        SistemaContabilDb db)
    {
        if (contaDto.Tipo is not ('R' or 'D'))
        {
            return TypedResults.BadRequest(new { message = "Tipo deve ser 'R' (Receita) ou 'D' (Despesa)" });
        }

        var conta = new Conta
        {
            IdContaContabil = await GetNextContaIdAsync(db),
            NomeContaContabil = contaDto.NomeContaContabil.Trim(),
            Tipo = contaDto.Tipo,
            ClienteIdCliente = contaDto.ClienteId
        };

        db.Contas.Add(conta);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/contas/{conta.IdContaContabil}", ToDto(conta));
    }

    static async Task<IResult> UpdateConta(
        [Description("id da conta que será atualizada.")] int id,
        UpdateContaRequest contaDto,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();
        var conta = await db.Contas.FindAsync(id);

        if (conta is null) return TypedResults.NotFound();

        if (contaDto.Tipo is not ('R' or 'D'))
        {
            return TypedResults.BadRequest(new { message = "Tipo deve ser 'R' (Receita) ou 'D' (Despesa)" });
        }

        conta.NomeContaContabil = contaDto.NomeContaContabil.Trim();
        conta.Tipo = contaDto.Tipo;
        conta.ClienteIdCliente = contaDto.ClienteId;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    static async Task<IResult> DeleteConta(
        [Description("id da conta que será deletada.")] int id,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();

        if (await db.RegistrosContabeis.AnyAsync(r => r.ContaId == id))
        {
            return TypedResults.BadRequest(new { message = "Conta possui registros contábeis vinculados" });
        }

        if (await db.Contas.FindAsync(id) is Conta conta)
        {
            db.Contas.Remove(conta);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    static ContaRequest ToDto(Conta conta)
    {
        return new ContaRequest
        {
            IdContaContabil = conta.IdContaContabil,
            NomeContaContabil = conta.NomeContaContabil,
            Tipo = conta.Tipo,
            ClienteId = conta.ClienteIdCliente,
            TipoDescricao = conta.GetTipoDescricao(),
            QuantidadeRegistros = conta.RegistrosContabeis.Count
        };
    }

    static async Task<int> GetNextContaIdAsync(SistemaContabilDb db)
    {
        var sequenceValue = await EndpointSequenceHelper.GetNextValueAsync(db, "conta_seq", "seq_conta");
        return sequenceValue > 0 ? sequenceValue : await db.Contas.Select(c => c.IdContaContabil).DefaultIfEmpty().MaxAsync() + 1;
    }

    #endregion
}
