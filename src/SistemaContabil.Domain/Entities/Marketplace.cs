using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContabil.Domain.Entities
{
    public class Marketplace
    {
        [Key]
        [Column("id_market")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nome_market")]
        public string Nome { get; set; }

        [Required]
        [Column("valor_comissao")]
        public decimal ValorComissao { get; set; }
    }
}
