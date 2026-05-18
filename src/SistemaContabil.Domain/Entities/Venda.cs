using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace SistemaContabil.Domain.Entities;

public class Venda
{
    [Key]
    public int IdVendas { get; set; }

    [Required]
    public DateTime DataVenda { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; }

    [Required]
    public int ClienteId { get; set; }

    [Required]
    public int RegContId { get; set; }

    public long? VendaEventoId { get; set; }

    [Required]
    [ForeignKey(nameof(Pagamento))]
    public int PagamentoId { get; set; }

    [Required]
    [ForeignKey(nameof(Marketplace))]
    public int MarketplaceId { get; set; }

    [ForeignKey(nameof(ClienteId))]
    public virtual Cliente Cliente { get; set; } = null!;

    [ForeignKey(nameof(RegContId))]
    public virtual RegistroContabil RegistroContabil { get; set; } = null!;

    /*
    [ForeignKey(nameof(PagamentoId))]
    public virtual Pagamento Pagamento { get; set; } = null!;
    */
    /*
    [ForeignKey(nameof(MarketplaceId))]
    public virtual Marketplace Marketplace { get; set; } = null!;
    */
    public bool IsValid()
    {
        return ClienteId > 0 && RegContId > 0;
    }
}
