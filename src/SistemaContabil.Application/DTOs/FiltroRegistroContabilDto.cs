namespace SistemaContabil.Application.DTOs;

public class FiltroRegistroContabilDto : SearchRequestDto
{
    public decimal? ValorMin { get; set; }

    public decimal? ValorMax { get; set; }

    public int? ContaId { get; set; }

    public int? CentroCustoId { get; set; }

    public DateTime? DataInicio { get; set; }

    public DateTime? DataFim { get; set; }
}
