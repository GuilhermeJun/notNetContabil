using System.ComponentModel;
using IdempotentAPI.MinimalAPI;
using Microsoft.EntityFrameworkCore;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Api.Endpoints;

public static class ClienteEndpoints
{
    public static void RegisterClienteEndpoints(this WebApplication app)
    {
        #region Routes

        var clientes = app.MapGroup("/clientes").WithTags("clientes");

        clientes.MapGet("/", GetAllClientes)
            .WithName("GetAllClientes")
            .WithSummary("Apresenta todos os clientes")
            .WithDescription("""
                         Apresenta todos os clientes cadastrados no sistema, incluindo ativos e inativos.
                         Não existe paginação ou filtros, assim que, se houver muitos registros,
                         a consulta pode demorar um certo tempo.
                         """)
            .Produces<List<ClienteDto>>();

        clientes.MapGet("/ativos", GetClientesAtivos)
            .WithName("GetClientesAtivos")
            .WithSummary("Apresenta todos os clientes ativos")
            .WithDescription("""
                         Apresenta todos os clientes cadastrados no sistema que estejam ativos.
                         Não existe paginação ou filtros, assim que, se houver muitos registros,
                         a consulta pode demorar um certo tempo.
                         """)
            .Produces<List<ClienteDto>>();

        clientes.MapGet("/{id:int}", GetCliente)
            .WithName("GetClienteById")
            .WithSummary("Busca um cliente por id")
            .WithDescription("Busca um cliente por id. Se o cliente não for encontrado, retorna um status code 404 (Not Found).")
            .Produces<ClienteDto>(200)
            .Produces(404);

        clientes.MapPost("/", CreateCliente)
            .WithName("CreateCliente")
            .WithSummary("Cria um novo cliente")
            .WithDescription("Cria um novo cliente. O id do cliente é gerado automaticamente pelo sistema.")
            .Produces<ClienteDto>(201)
            .Produces(400)
            .AddEndpointFilter<IdempotentAPIEndpointFilter>();

        clientes.MapPut("/{id:int}", UpdateCliente)
            .WithName("UpdateClienteById")
            .WithSummary("Atualiza um cliente pelo seu id")
            .WithDescription("Atualiza um cliente pelo seu id. Se o cliente não for encontrado, retorna um status code 404 (Not Found).")
            .Produces(204)
            .Produces(404)
            .Produces(400);

        clientes.MapDelete("/{id:int}", DeleteCliente)
            .WithName("DeleteClienteById")
            .WithSummary("Delete um cliente pelo seu id")
            .WithDescription("Deleta um cliente pelo seu id. Se o cliente não for encontrado, retorna um status code 404 (Not Found).")
            .Produces(204)
            .Produces(404)
            .Produces(400)
            .Accepts<ClienteDto>("application/json", "application/xml");

        #endregion
    }

    #region Services

    static async Task<IResult> GetAllClientes(SistemaContabilDb db)
    {
        return TypedResults.Ok(await db.Clientes
            .Select(x => new ClienteDto(x))
            .ToListAsync());
    }

    static async Task<IResult> GetClientesAtivos(SistemaContabilDb db)
    {
        return TypedResults.Ok(await db.Clientes
            .Where(c => c.Ativo == 'S')
            .Select(x => new ClienteDto(x))
            .ToListAsync());
    }

    static async Task<IResult> GetCliente(
        [Description("id do cliente que será buscado.")] int id,
        SistemaContabilDb db)
    {
        return await db.Clientes.FindAsync(id)
            is Cliente cliente ? TypedResults.Ok(new ClienteDto(cliente)) : TypedResults.NotFound();
    }

    static async Task<IResult> CreateCliente(
        [Description("Cliente a ser cadastrado.")] CriarClienteDto clienteDto,
        SistemaContabilDb db)
    {
        if (await db.Clientes.AnyAsync(c => c.Cpf == clienteDto.Cpf))
        {
            return TypedResults.BadRequest(new { message = "Já existe um cliente com este CPF/CNPJ" });
        }

        if (await db.Clientes.AnyAsync(c => c.Email == clienteDto.Email))
        {
            return TypedResults.BadRequest(new { message = "Já existe um cliente com este email" });
        }

        var cliente = new Cliente
        {
            Id = await GetNextClienteIdAsync(db),
            Nome = clienteDto.Nome.Trim(),
            Cpf = clienteDto.Cpf.Trim(),
            Email = clienteDto.Email.Trim(),
            Ativo = clienteDto.AtivoChar,
            DataCadastro = DateTime.Now
        };

        db.Clientes.Add(cliente);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/clientes/{cliente.Id}", new ClienteDto(cliente));
    }

    static async Task<IResult> UpdateCliente(
        [Description("id do cliente que será atualizado.")] int id,
        AtualizarClienteDto clienteDto,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();
        var cliente = await db.Clientes.FindAsync(id);

        if (cliente is null) return TypedResults.NotFound();

        var ativo = !string.IsNullOrEmpty(clienteDto.Ativo) ? clienteDto.Ativo[0] : 'S';
        if (ativo is not ('S' or 'N'))
        {
            return TypedResults.BadRequest(new { message = "Ativo deve ser 'S' (Ativo) ou 'N' (Inativo)" });
        }

        if (await db.Clientes.AnyAsync(c => c.Id != id && c.Email == clienteDto.Email))
        {
            return TypedResults.BadRequest(new { message = "Já existe outro cliente com este email" });
        }

        cliente.Nome = clienteDto.Nome.Trim();
        cliente.Email = clienteDto.Email.Trim();
        cliente.Ativo = ativo;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    static async Task<IResult> DeleteCliente(
        [Description("id do cliente que será deletado.")] int id,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();

        if (await db.Clientes.FindAsync(id) is Cliente cliente)
        {
            db.Clientes.Remove(cliente);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    static async Task<int> GetNextClienteIdAsync(SistemaContabilDb db)
    {
        var result = await db.Database
            .SqlQueryRaw<int>("SELECT cliente_seq.NEXTVAL FROM DUAL")
            .ToListAsync();

        return result.FirstOrDefault();
    }

    #endregion
}
