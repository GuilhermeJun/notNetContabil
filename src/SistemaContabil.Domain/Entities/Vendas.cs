using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace SistemaContabil.Domain.Entities;

public class Vendas
{
    [Key]
    public int IdVendas { get; set; }

    [Required]
    public int ClienteId { get; set; }

    [Required]
    public int RegContId { get; set; }

    public long? VendaEventoId { get; set; }

    [ForeignKey(nameof(ClienteId))]
    public virtual Cliente Cliente { get; set; } = null!;

    [ForeignKey(nameof(RegContId))]
    public virtual RegistroContabil RegistroContabil { get; set; } = null!;

    /// Valida se a venda está válida para operações
    /// <returns>True se válido, False caso contrário</returns>
    public bool IsValid()
    {
        return ClienteId > 0 && RegContId > 0;
    }
}
