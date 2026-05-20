using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContabil.Domain.Entities
{
    public class ItemVenda
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        [ForeignKey(nameof(Produto))]
        public int ProdutoId { get; set; }

        [Required]
        [ForeignKey(nameof(Venda))]
        public int VendaId { get; set; }
    }
}
