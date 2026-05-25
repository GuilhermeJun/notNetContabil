using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContabil.Domain.Entities;

[Table("conta_contabil")]
public class Conta
{
    [Key]
    [Column("id_conta")]
    public int IdConta { get; set; }

    [Required]
    [StringLength(70)]
    [Column("nome_conta")]
    public string NomeConta { get; set; } = string.Empty;

    [Required]
    [StringLength(1)]
    [Column("tipo_conta")]
    public char Tipo { get; set; }

    [Column("cliente_id_cliente")]
    public int? ClienteIdCliente { get; set; }

    [ForeignKey(nameof(ClienteIdCliente))]
    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<RegistroContabil> RegistrosContabeis { get; set; } = new List<RegistroContabil>();

    public string GetTipoDescricao()
    {
        return Tipo == 'R' ? "Receita" : Tipo == 'D' ? "Despesa" : string.Empty;
    }
}
