namespace SistemaContabil.Application.DTOs;

public class VendasDto
{
    public int IdVendas { get; set; }
    public int ClienteIdCliente { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public int RegContIdRegCont { get; set; }
    public decimal Valor { get; set; }
    public long? VendaEventoIdEvento { get; set; }
}

public class CriarVendasDto
{
    public int ClienteIdCliente { get; set; }
    public int RegContIdRegCont { get; set; }
    public long? VendaEventoIdEvento { get; set; }
}

public class AtualizarVendasDto
{
    public int ClienteIdCliente { get; set; }
    public int RegContIdRegCont { get; set; }
    public long? VendaEventoIdEvento { get; set; }
}
