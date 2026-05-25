using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContabil.Domain.Entities
{
    [Table("pagamento")]
    public class Pagamento
    {
        [Key]
        [Column("id_pag")]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Column("metodo_pag")]
        public string MetodoPagamento { get; set; }

        [Required]
        [Column("data_pag")]
        public DateTime DataPagamento { get; set; } = DateTime.Now;

        [Column("taxa_pag")]
        public decimal? Taxa { get; set; }

        [Required]
        [StringLength(20)]
        [Column("status")]
        public string Status { get; set; }
    }
}
