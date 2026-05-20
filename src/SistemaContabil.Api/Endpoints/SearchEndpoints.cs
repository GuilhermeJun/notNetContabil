using Microsoft.AspNetCore.Mvc;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Application.Services;
using SistemaContabil.Web.Helpers;

namespace SistemaContabil.Web.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/search")
            .WithTags("Search")
            .WithDescription("Endpoints de busca paginada com filtros e ordenação");

        group.MapGet("/centrocusto", async (
            ICentroCustoAppService service,
            HttpContext httpContext,
            [FromQuery] string? nome = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string sortOrder = "asc") =>
        {
            var filtro = new FiltroCentroCustoDto
            {
                Nome = nome,
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            filtro.Validate();
            var result = await service.SearchAsync(filtro);
            
            // Adicionar links HATEOAS
            HateoasHelper.AddHateoasLinks(result, httpContext, "centrocusto", filtro);

            return Results.Ok(result);
        })
        .WithName("SearchCentroCusto")
        .WithSummary("Busca paginada de centros de custo")
        .Produces<PagedResultDto<CentroCustoDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        // Busca de Contas
        group.MapGet("/conta", async (
            IContaAppService service,
            HttpContext httpContext,
            [FromQuery] string? nome = null,
            [FromQuery] char? tipo = null,
            [FromQuery] int? clienteId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string sortOrder = "asc") =>
        {
            var filtro = new FiltroContaDto
            {
                Nome = nome,
                Tipo = tipo,
                ClienteId = clienteId,
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            filtro.Validate();
            var result = await service.SearchAsync(filtro);
            
            // Adicionar links HATEOAS
            HateoasHelper.AddHateoasLinks(result, httpContext, "conta", filtro);

            return Results.Ok(result);
        })
        .WithName("SearchConta")
        .WithSummary("Busca paginada de contas contábeis")
        .Produces<PagedResultDto<ContaDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        // Busca de Registros Contábeis
        group.MapGet("/registrocontabil", async (
            IRegistroContabilAppService service,
            HttpContext httpContext,
            [FromQuery] decimal? valorMin = null,
            [FromQuery] decimal? valorMax = null,
            [FromQuery] int? contaId = null,
            [FromQuery] int? centroCustoId = null,
            [FromQuery] DateTime? dataInicio = null,
            [FromQuery] DateTime? dataFim = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string sortOrder = "asc") =>
        {
            var filtro = new FiltroRegistroContabilDto
            {
                ValorMin = valorMin,
                ValorMax = valorMax,
                ContaId = contaId,
                CentroCustoId = centroCustoId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            filtro.Validate();
            var result = await service.SearchAsync(filtro);
            
            // Adicionar links HATEOAS
            HateoasHelper.AddHateoasLinks(result, httpContext, "registrocontabil", filtro);

            return Results.Ok(result);
        })
        .WithName("SearchRegistroContabil")
        .WithSummary("Busca paginada de registros contábeis")
        .Produces<PagedResultDto<RegistroContabilDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        // Busca de Clientes
        group.MapGet("/cliente", async (
            IClienteAppService service,
            HttpContext httpContext,
            [FromQuery] string? nome = null,
            [FromQuery] string? cpfCnpj = null,
            [FromQuery] char? ativo = null,
            [FromQuery] string? email = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string sortOrder = "asc") =>
        {
            var filtro = new FiltroClienteDto
            {
                Nome = nome,
                CpfCnpj = cpfCnpj,
                Ativo = ativo,
                Email = email,
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            filtro.Validate();
            var result = await service.SearchAsync(filtro);
            
            // Adicionar links HATEOAS
            HateoasHelper.AddHateoasLinks(result, httpContext, "cliente", filtro);

            return Results.Ok(result);
        })
        .WithName("SearchCliente")
        .WithSummary("Busca paginada de clientes")
        .Produces<PagedResultDto<ClienteDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        // Busca de Vendas
        group.MapGet("/vendas", async (
            IVendasAppService service,
            HttpContext httpContext,
            [FromQuery] int? clienteId = null,
            [FromQuery] int? regContId = null,
            [FromQuery] long? vendaEventoId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] string sortOrder = "asc") =>
        {
            var filtro = new FiltroVendasDto
            {
                ClienteId = clienteId,
                RegContId = regContId,
                VendaEventoId = vendaEventoId,
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            filtro.Validate();
            var result = await service.SearchAsync(filtro);
            
            // Adicionar links HATEOAS
            HateoasHelper.AddHateoasLinks(result, httpContext, "vendas", filtro);

            return Results.Ok(result);
        })
        .WithName("SearchVendas")
        .WithSummary("Busca paginada de vendas")
        .Produces<PagedResultDto<VendasDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
