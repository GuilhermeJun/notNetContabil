using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContabil.Domain.Entities
{
    [Table("item_venda")]
    public class ItemVenda
    {
        [Key]
        [Column("id_item")]
        public int Id { get; set; }

        [Required]
        [Column("quant")]
        public int Quantidade { get; set; }

        [Required]
        [Column("valor_unit")]
        public decimal ValorUnitario { get; set; }

        [Required]
        [Column("produto_id_prod")]
        public int ProdutoId { get; set; }

        [ForeignKey(nameof(ProdutoId))]
        public virtual Produto Produto { get; set; }

        [Required]
        [Column("vendas_id_vendas")]
        public int VendaId { get; set; }

        [ForeignKey(nameof(VendaId))]
        public virtual Venda Venda { get; set; }
    }
}
