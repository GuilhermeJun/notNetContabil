using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Test;

public class ClienteHateoasIntegrationTest : IClassFixture<ClienteApiFactory>
{
    private readonly HttpClient _client;

    public ClienteHateoasIntegrationTest(ClienteApiFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public async Task GetClienteById_WhenClienteExists_ShouldReturnClienteWithHateoasLinks()
    {
        // Arrange
        const int clienteId = 1;

        // Act
        var response = await _client.GetAsync($"/clientes/{clienteId}");
        var cliente = await response.Content.ReadFromJsonAsync<ClienteHateoasResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(cliente);
        Assert.Equal(clienteId, cliente.Id);
        Assert.Equal("Cliente Teste", cliente.Nome);
        Assert.Contains(cliente.Links, link =>
            link.Rel == "self" &&
            link.Method == HttpMethods.Get &&
            link.Href.EndsWith($"/clientes/{clienteId}"));
        Assert.Contains(cliente.Links, link =>
            link.Rel == "clientes" &&
            link.Method == HttpMethods.Get &&
            link.Href.EndsWith("/clientes"));
        Assert.Contains(cliente.Links, link =>
            link.Rel == "atualizar" &&
            link.Method == HttpMethods.Put &&
            link.Href.EndsWith($"/clientes/{clienteId}"));
        Assert.Contains(cliente.Links, link =>
            link.Rel == "excluir" &&
            link.Method == HttpMethods.Delete &&
            link.Href.EndsWith($"/clientes/{clienteId}"));
    }

    [Fact]
    public async Task GetClienteById_WhenClienteDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        const int clienteId = 999;

        // Act
        var response = await _client.GetAsync($"/clientes/{clienteId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

public class ClienteApiFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"SistemaContabilTests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions>();
            services.RemoveAll<DbContextOptions<SistemaContabilDb>>();
            services.RemoveAll<IDbContextOptionsConfiguration<SistemaContabilDb>>();
            services.AddDbContext<SistemaContabilDb>(options =>
                options.UseInMemoryDatabase(_databaseName));

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SistemaContabilDb>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Clientes.Add(new Cliente
            {
                Id = 1,
                Nome = "Cliente Teste",
                DataCadastro = new DateTime(2026, 5, 24),
                Cpf = "12345678901",
                Email = "cliente@teste.com",
                Senha = "senha-teste",
                Ativo = 'S'
            });

            db.SaveChanges();
        });
    }
}
