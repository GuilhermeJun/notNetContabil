using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContabil.Domain.Entities;

public class Venda
{
    [Key]
    [Column("id_vendas")]
    public int Id { get; set; }

    [Required]
    [Column("data_venda")]
    public DateTime DataVenda { get; set; }

    [Required]
    [StringLength(20)]
    [Column("status_venda")]
    public string Status { get; set; }

    [Required]
    [Column("valor_bruto")]
    public decimal ValorBruto { get; set; }

    [Required]
    [Column("valor_desconto")]
    public decimal ValorDesconto { get; set; }

    [Required]
    [Column("valor_frete")]
    public decimal ValorFrete { get; set; }

    [Required]
    [Column("valor_liquido")]
    public decimal ValorLiquido { get; set; }

    [Required]
    [Column("valor_total")]
    public decimal ValorTotal { get; set; }

    [Required]
    [Column("cliente_id_cliente")]
    public int ClienteId { get; set; }

    [ForeignKey(nameof(ClienteId))]
    public virtual Cliente Cliente { get; set; }

    [Required]
    [Column("reg_cont_id_reg_cont")]
    public int RegContId { get; set; }

    [ForeignKey(nameof(RegContId))]
    public virtual RegistroContabil RegCont { get; set; }

    [Column("pagamento_id_pag")]
    public int? PagamentoId { get; set; }

    [ForeignKey(nameof(PagamentoId))]
    public virtual Pagamento? Pagamento { get; set; }

    [Required]
    [Column("marketplace_id_market")]
    public int MarketplaceId { get; set; }

    [ForeignKey(nameof(MarketplaceId))]
    public virtual Marketplace Marketplace { get; set; }

    // Aliases for backward compatibility with existing code that uses different property names
    [Column("id_vendas")]
    public int IdVendas { get => Id; set => Id = value; }

    [Column("venda_evento_id")]
    public long? VendaEventoId { get; set; }

    // Compatibility navigation
    public RegistroContabil RegistroContabil { get => RegCont; set => RegCont = value; }
}
