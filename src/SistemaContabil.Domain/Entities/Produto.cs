using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContabil.Domain.Entities
{
    public class Produto
    {
        [Key]
        [Column("id_prod")]
        public int Id { get; set; }

        [Required]
        [Column("nome_prod")]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [Column("descr")]
        [StringLength(250)]
        public string Descricao { get; set; }

        [Required]
        [Column("preco")]
        public decimal Preco { get; set; }

        [Required]
        [Column("estoque")]
        public int Estoque { get; set; }
    }
}
