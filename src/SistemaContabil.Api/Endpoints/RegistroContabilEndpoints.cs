using System.ComponentModel;
using IdempotentAPI.MinimalAPI;
using Microsoft.EntityFrameworkCore;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Api.Endpoints;

public static class RegistroContabilEndpoints
{
    public static void RegisterRegistroContabilEndpoints(this WebApplication app)
    {
        #region Routes

        var registros = app.MapGroup("/registros-contabeis").WithTags("registros-contabeis");

        registros.MapGet("/", GetAllRegistrosContabeis)
            .WithName("GetAllRegistrosContabeis")
            .WithSummary("Apresenta todos os registros contábeis")
            .Produces<List<RegistroContabilDto>>();

        registros.MapGet("/{id:int}", GetRegistroContabil)
            .WithName("GetRegistroContabilById")
            .WithSummary("Busca um registro contábil por id")
            .Produces<RegistroContabilDto>(200)
            .Produces(404);

        registros.MapPost("/", CreateRegistroContabil)
            .WithName("CreateRegistroContabil")
            .WithSummary("Cria um novo registro contábil")
            .Produces<RegistroContabilDto>(201)
            .Produces(400)
            .AddEndpointFilter<IdempotentAPIEndpointFilter>();

        registros.MapPut("/{id:int}", UpdateRegistroContabil)
            .WithName("UpdateRegistroContabilById")
            .WithSummary("Atualiza um registro contábil pelo seu id")
            .Produces(204)
            .Produces(404)
            .Produces(400);

        registros.MapDelete("/{id:int}", DeleteRegistroContabil)
            .WithName("DeleteRegistroContabilById")
            .WithSummary("Deleta um registro contábil pelo seu id")
            .Produces(204)
            .Produces(404)
            .Produces(400);

        #endregion
    }

    #region Services

    static async Task<IResult> GetAllRegistrosContabeis(SistemaContabilDb db)
    {
        return TypedResults.Ok(await db.RegistrosContabeis
            .Include(r => r.Conta)
            .Select(r => ToDto(r))
            .ToListAsync());
    }

    static async Task<IResult> GetRegistroContabil(
        [Description("id do registro contábil que será buscado.")] int id,
        SistemaContabilDb db)
    {
        var registro = await db.RegistrosContabeis
            .Include(r => r.Conta)
            .FirstOrDefaultAsync(r => r.IdRegCont == id);

        return registro is null ? TypedResults.NotFound() : TypedResults.Ok(ToDto(registro));
    }

    static async Task<IResult> CreateRegistroContabil(
        [Description("Registro contábil a ser cadastrado.")] CriarRegistroContabilDto registroDto,
        SistemaContabilDb db)
    {
        var validation = await ValidateReferencesAsync(registroDto.ContaId, registroDto.CentroCustoId, db);
        if (validation is not null) return validation;

        var registro = new RegistroContabil
        {
            IdRegCont = await GetNextRegistroContabilIdAsync(db),
            Valor = registroDto.Valor,
            ContaId = registroDto.ContaId,
            CentroCustoId = registroDto.CentroCustoId,
            DataCriacao = DateTime.Now
        };

        db.RegistrosContabeis.Add(registro);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/registros-contabeis/{registro.IdRegCont}", ToDto(registro));
    }

    static async Task<IResult> UpdateRegistroContabil(
        [Description("id do registro contábil que será atualizado.")] int id,
        AtualizarRegistroContabilDto registroDto,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();
        var registro = await db.RegistrosContabeis.FindAsync(id);

        if (registro is null) return TypedResults.NotFound();

        var validation = await ValidateReferencesAsync(registroDto.ContaId, registroDto.CentroCustoId, db);
        if (validation is not null) return validation;

        registro.Valor = registroDto.Valor;
        registro.ContaId = registroDto.ContaId;
        registro.CentroCustoId = registroDto.CentroCustoId;
        registro.DataAtualizacao = DateTime.Now;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    static async Task<IResult> DeleteRegistroContabil(
        [Description("id do registro contábil que será deletado.")] int id,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();

        if (await db.Vendas.AnyAsync(v => v.RegContId == id))
        {
            return TypedResults.BadRequest(new { message = "Registro contábil possui vendas vinculadas" });
        }

        if (await db.RegistrosContabeis.FindAsync(id) is RegistroContabil registro)
        {
            db.RegistrosContabeis.Remove(registro);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    static async Task<IResult?> ValidateReferencesAsync(int contaId, int centroCustoId, SistemaContabilDb db)
    {
        if (!await db.Contas.AnyAsync(c => c.IdContaContabil == contaId))
        {
            return TypedResults.BadRequest(new { message = "Conta não encontrada" });
        }
        return null;
    }

    static RegistroContabilDto ToDto(RegistroContabil registro)
    {
        return new RegistroContabilDto
        {
            IdRegCont = registro.IdRegCont,
            Valor = registro.Valor,
            ContaId = registro.ContaId,
            CentroCustoId = registro.CentroCustoId,
            NomeConta = registro.Conta?.NomeContaContabil ?? string.Empty,
            DataCriacao = registro.DataCriacao,
            DataAtualizacao = registro.DataAtualizacao
        };
    }

    static async Task<int> GetNextRegistroContabilIdAsync(SistemaContabilDb db)
    {
        var sequenceValue = await EndpointSequenceHelper.GetNextValueAsync(db, "reg_cont_seq", "registro_contabil_seq", "seq_registro_contabil");
        return sequenceValue > 0 ? sequenceValue : await db.RegistrosContabeis.Select(r => r.IdRegCont).DefaultIfEmpty().MaxAsync() + 1;
    }

    #endregion
}
