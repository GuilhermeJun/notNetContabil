using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContabil.Domain.Entities
{
    internal class ItemVenda
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
