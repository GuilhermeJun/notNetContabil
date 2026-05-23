using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContabil.Domain.Entities;

public class RegistroContabil
{
    [Key]
    [Column("id_reg_cont")]
    public int Id { get; set; }

    [Required]
    [Column("valor")]
    public decimal Valor { get; set; }

    [Column("data_lanca")]
    public DateTime DataLancamento { get; set; } = DateTime.Now;

    [Column("data_atualizacao")]
    public DateTime? DataAtualizacao { get; set; }

    [Required]
    [Column("conta_contabil_id_conta")]
    public int ContaId { get; set; }

    [ForeignKey(nameof(ContaId))]
    public virtual Conta Conta { get; set; }

    // Backwards compatible aliases
    [Column("id_reg_cont")]
    public int IdRegCont { get => Id; set => Id = value; }

    [Column("data_criacao")]
    public DateTime DataCriacao { get => DataLancamento; set => DataLancamento = value; }

    // CentroCustoId is not present in DB schema; keep nullable for compatibility
    public int? CentroCustoId { get; set; }

}
