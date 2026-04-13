namespace SistemaContabil.Application.DTOs;

public class VendasDto
{
    public int IdVendas { get; set; }
    public int ClienteId { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public int RegContId { get; set; }
    public decimal Valor { get; set; }
    public long? VendaEventoId { get; set; }
}

public class CriarVendasDto
{
    public int ClienteId { get; set; }
    public int RegContId { get; set; }
    public long? VendaEventoId { get; set; }
}

public class AtualizarVendasDto
{
    public int ClienteId { get; set; }
    public int RegContId { get; set; }
    public long? VendaEventoId { get; set; }
}
