using System.ComponentModel.DataAnnotations;
using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Application.DTOs;

public class PagamentoResponse
{
    public int Id { get; set; }
    public string MetodoPagamento { get; set; } = string.Empty;
    public DateTime DataPagamento { get; set; }
    public decimal? Taxa { get; set; }
    public string Status { get; set; } = string.Empty;

    public PagamentoResponse() { }

    public PagamentoResponse(Pagamento pagamento)
    {
        Id = pagamento.Id;
        MetodoPagamento = pagamento.MetodoPagamento;
        DataPagamento = pagamento.DataPagamento;
        Taxa = pagamento.Taxa;
        Status = pagamento.Status;
    }
}

public class CreatePagamentoRequest
{
    [Required]
    [StringLength(30)]
    public string MetodoPagamento { get; set; } = string.Empty;

    public decimal? Taxa { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = string.Empty;
}

public class UpdatePagamentoRequest
{
    [Required]
    [StringLength(30)]
    public string MetodoPagamento { get; set; } = string.Empty;

    public decimal? Taxa { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = string.Empty;
}
