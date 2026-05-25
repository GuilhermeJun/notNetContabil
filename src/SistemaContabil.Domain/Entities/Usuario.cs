using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContabil.Domain.Entities
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nome_usuario")]
        public string Nome { get; set; }

        [Required]
        [Column("data_cadastro")]
        public DateTime DataCadastro { get; set; }

        [Required]
        [StringLength(100)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        [Column("senha")]
        public string Senha { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public char Ativo { get; set; }
    }
}
