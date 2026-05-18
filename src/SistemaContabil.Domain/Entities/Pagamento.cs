using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContabil.Domain.Entities
{
    internal class Pagamento
    {
        [Key]
        public int IdPagamento { get; set; }

        [Required]
        [StringLength(30)]
        public string MetodoPagamento { get; set; }
    }
}
