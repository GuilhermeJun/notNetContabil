using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace SistemaContabil.Test
{
    public class ClienteTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ClienteTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ObterTodos_ShouldReturn200OkWithAlist()
        {
            var response = await _client.GetAsync("/api/Cliente");

            response.EnsureSuccessStatusCode();
            var clientes = await response.Content.ReadFromJsonAsync<List<Cliente>>();
            Assert.IsType<List<Cliente>>(clientes);
        }

        [Theory]
        [InlineData(1, HttpStatusCode.OK)]
        [InlineData(-999, HttpStatusCode.NotFound)]
        public async Task ObterPorId_ShouldReturnCorrectStatusCode(int id, HttpStatusCode expectedStatusCode)
        {
            var response = await _client.GetAsync($"/api/Cliente/{id}");
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task _Criar_WithoutIdempotencyKey_ShouldReturn400BadRequest()
        {
            var newCliente = new ClienteDto()
            {
                NomeCliente = "Teste Cliente",
                CpfCnpj = "12345678901",
                Email = "jun@teste.com",
            };

            _client.DefaultRequestHeaders.Remove("Idempotency-Key");

            var response = await _client.PostAsJsonAsync("/api/Cliente", newCliente);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
