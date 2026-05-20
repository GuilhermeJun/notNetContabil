using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContabil.Domain.Entities
{
    public class Marketplace
    {
        [Key]
        public int IdMarketplace { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeMarketplace { get; set; }
        
        [Required]
        public float Comissao { get; set; }
    }
}
