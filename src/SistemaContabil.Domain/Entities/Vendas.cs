using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace SistemaContabil.Domain.Entities;

public class Vendas
{
    [Key]
    public int IdVendas { get; set; }

    [Required]
    public int ClienteIdCliente { get; set; }

    [Required]
    public int RegContIdRegCont { get; set; }

    public long? VendaEventoIdEvento { get; set; }

    [ForeignKey(nameof(ClienteIdCliente))]
    public virtual Cliente Cliente { get; set; } = null!;

    [ForeignKey(nameof(RegContIdRegCont))]
    public virtual RegistroContabil RegistroContabil { get; set; } = null!;

    /// Valida se a venda está válida para operações
    /// <returns>True se válido, False caso contrário</returns>
    public bool IsValid()
    {
        return ClienteIdCliente > 0 && RegContIdRegCont > 0;
    }
}
