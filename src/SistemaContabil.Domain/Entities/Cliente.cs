using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContabil.Domain.Entities;

public class Cliente
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Column("nome_cliente")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [Column("data_cadastro")]
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    [Required]
    [StringLength(14)]
    [Column("cpf")]
    public string Cpf { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [EmailAddress]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("senha")]
    public string Senha { get; set; } = string.Empty;

    [Required]
    [StringLength(1)]
    [Column("ativo")]
    public char Ativo { get; set; } = 'S';

    public virtual ICollection<Conta> Contas { get; set; } = new List<Conta>();

    public virtual ICollection<Venda> Vendas { get; set; } = new List<Venda>();

}
