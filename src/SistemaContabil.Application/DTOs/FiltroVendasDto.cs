namespace SistemaContabil.Application.DTOs;

public class FiltroVendasDto : SearchRequestDto
{
    public int? ClienteId { get; set; }

    public int? RegContId { get; set; }

    public long? VendaEventoId { get; set; }
}
