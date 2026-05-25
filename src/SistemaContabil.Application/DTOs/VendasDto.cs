namespace SistemaContabil.Application.DTOs;

public class VendaRequest
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public int RegContId { get; set; }
    public decimal Valor { get; set; }
}

public class CreateVendaRequest
{
    public int ClienteId { get; set; }
    public int RegContId { get; set; }
    public int PagamentoId { get; set; }
    public int MarketplaceId { get; set; }
    public string? Status { get; set; }
}
