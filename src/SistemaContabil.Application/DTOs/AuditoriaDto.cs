using System.ComponentModel.DataAnnotations;
using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Application.DTOs;

public class AuditoriaResponse
{
    public int Id { get; set; }
    public string Tabela { get; set; } = string.Empty;
    public string Operacao { get; set; } = string.Empty;
    public DateTime DataOperacao { get; set; }
    public string DadosOld { get; set; } = string.Empty;
    public string DadosNew { get; set; } = string.Empty;

    public AuditoriaResponse() { }
}

public class CreateAuditoriaRequest
{
    [Required]
    public string Tabela { get; set; } = string.Empty;

    [Required]
    public string Operacao { get; set; } = string.Empty;

    public string DadosOld { get; set; } = string.Empty;
    public string DadosNew { get; set; } = string.Empty;
}
