using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Infrastructure.Data;
using System.ComponentModel;

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
            .Produces<List<ClienteResponse>>();

        clientes.MapGet("/ativos", GetClientesAtivos)
            .WithName("GetClientesAtivos")
            .WithSummary("Apresenta todos os clientes ativos")
            .WithDescription("""
                         Apresenta todos os clientes cadastrados no sistema que estejam ativos.
                         Não existe paginação ou filtros, assim que, se houver muitos registros,
                         a consulta pode demorar um certo tempo.
                         """)
            .Produces<List<ClienteResponse>>();

        clientes.MapGet("/{id:int}", GetCliente)
            .WithName("GetClienteById")
            .WithSummary("Busca um cliente por id")
            .WithDescription("Busca um cliente por id. Se o cliente não for encontrado, retorna um status code 404 (Not Found).")
            .Produces<ClienteHateoasResponse>(200)
            .Produces(404);

        clientes.MapPost("/", CreateCliente)
            .WithName("CreateCliente")
            .WithSummary("Cria um novo cliente")
            .WithDescription("Cria um novo cliente. O id do cliente é gerado automaticamente pelo sistema.")
            .Produces<ClienteResponse>(201)
            .Produces(400);

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
            .Accepts<ClienteResponse>("application/json", "application/xml");

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
            .Select(x => new ClienteResponse(x))
            .ToListAsync());
    }

    static async Task<IResult> GetCliente(
        [Description("id do cliente que será buscado.")] int id,
        SistemaContabilDb db,
        LinkGenerator linkGenerator,
        HttpContext httpContext)
    {
        var cliente = await db.Clientes.FindAsync(id);

        if (cliente is null)
        {
            return TypedResults.NotFound();
        }

        var response = new ClienteHateoasResponse(cliente)
        {
            Links =
            [
                CreateLink("self", "GetClienteById", HttpMethods.Get, new { id = cliente.Id }),
                CreateLink("clientes", "GetAllClientes", HttpMethods.Get),
                CreateLink("atualizar", "UpdateClienteById", HttpMethods.Put, new { id = cliente.Id }),
                CreateLink("excluir", "DeleteClienteById", HttpMethods.Delete, new { id = cliente.Id })
            ]
        };

        return TypedResults.Ok(response);

        HateoasLinkDto CreateLink(string rel, string routeName, string method, object? values = null)
        {
            var href = linkGenerator.GetUriByName(httpContext, routeName, values)
                ?? linkGenerator.GetPathByName(routeName, values)
                ?? string.Empty;

            return new HateoasLinkDto(rel, href, method);
        }
    }

    static async Task<IResult> CreateCliente(
        [Description("Cliente a ser cadastrado.")] CreateClienteRequest clienteDto,
        SistemaContabilDb db)
    {
        /*
        if (await db.Clientes.AnyAsync(c => c.Cpf == clienteDto.Cpf))
        {
            return TypedResults.BadRequest(new { message = "Já existe um cliente com este CPF" });
        }

        if (await db.Clientes.AnyAsync(c => c.Email == clienteDto.Email))
        {
            return TypedResults.BadRequest(new { message = "Já existe um cliente com este email" });
        }
        */

        var cliente = new Cliente
        {
            Nome = clienteDto.Nome.Trim(),
            DataCadastro = DateTime.Now,
            Cpf = clienteDto.Cpf.Trim(),
            Email = clienteDto.Email.Trim(),
            Senha = clienteDto.Senha.Trim(),
            Ativo = clienteDto.Ativo
        };

        db.Clientes.Add(cliente);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/clientes/{cliente.Id}", new ClienteResponse(cliente));
    }

    static async Task<IResult> UpdateCliente(
        [Description("id do cliente que será atualizado.")] int id,
        UpdateClienteRequest clienteDto,
        HttpContext http)
    {
        var db = http.RequestServices.GetRequiredService<SistemaContabilDb>();
        var cliente = await db.Clientes.FindAsync(id);

        if (cliente is null) return TypedResults.NotFound();

        // Se não foi informado no DTO, mantém o valor atual do cliente
        /*
        var ativo = clienteDto.Ativo != '\0' ? clienteDto.Ativo : cliente.Ativo;
        if (ativo != 'S' && ativo != 'N')
        {
            return TypedResults.BadRequest(new { message = "Ativo deve ser 'S' (Ativo) ou 'N' (Inativo)" });
        }

        if (await db.Clientes.AnyAsync(c => c.Id != id && c.Email == clienteDto.Email))
        {
            return TypedResults.BadRequest(new { message = "Já existe outro cliente com este email" });
        }
        */

        cliente.Nome = clienteDto.Nome.Trim();
        cliente.Email = clienteDto.Email.Trim();
        cliente.Ativo = clienteDto.Ativo;

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

    #endregion
}
