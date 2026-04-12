using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContabil.Domain.Entities;

public class RegistroContabil
{
    [Key]
    public int IdRegCont { get; set; }

    [Required]
    [Column(TypeName = "decimal(9,2)")]
    public decimal Valor { get; set; }

    [Required]
    public int ContaIdConta { get; set; }

    [Required]
    public int CentroCustoId { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public DateTime? DataAtualizacao { get; set; }

    [ForeignKey(nameof(ContaIdConta))]
    public virtual Conta Conta { get; set; } = null!;

    [ForeignKey(nameof(CentroCustoId))]
    public virtual CentroCusto CentroCusto { get; set; } = null!;

    public virtual ICollection<Vendas> Vendas { get; set; } = new List<Vendas>();

    /// Valida se o registro contábil está válido para operações
    /// <returns>True se válido, False caso contrário</returns>
    public bool IsValid()
    {
        return Valor > 0 && 
               Valor <= 999999.99m &&
               ContaIdConta > 0 &&
               CentroCustoId > 0;
    }

    /// Atualiza o valor do registro contábil
    /// <param name="novoValor">Novo valor para o registro</param>
    /// <exception cref="ArgumentException">Lançada quando o valor é inválido</exception>
    public void AtualizarValor(decimal novoValor)
    {
        if (novoValor <= 0)
            throw new ArgumentException("Valor deve ser maior que zero", nameof(novoValor));
        
        if (novoValor > 999999.99m)
            throw new ArgumentException("Valor não pode ser maior que 999.999,99", nameof(novoValor));

        Valor = novoValor;
        DataAtualizacao = DateTime.Now;
    }

    /// Atualiza a conta associada ao registro
    /// <param name="contaId">ID da nova conta</param>
    /// <exception cref="ArgumentException">Lançada quando o ID é inválido</exception>
    public void AtualizarConta(int contaId)
    {
        if (contaId <= 0)
            throw new ArgumentException("ID da conta deve ser maior que zero", nameof(contaId));

        ContaIdConta = contaId;
        DataAtualizacao = DateTime.Now;
    }

    /// Atualiza o centro de custo associado ao registro
    /// <param name="centroCustoId">ID do novo centro de custo</param>
    /// <exception cref="ArgumentException">Lançada quando o ID é inválido</exception>
    public void AtualizarCentroCusto(int centroCustoId)
    {
        if (centroCustoId <= 0)
            throw new ArgumentException("ID do centro de custo deve ser maior que zero", nameof(centroCustoId));

        CentroCustoId = centroCustoId;
        DataAtualizacao = DateTime.Now;
    }

    /// Retorna uma descrição resumida do registro
    public string GetDescricao()
    {
        return $"Registro: {IdRegCont} - Valor: {Valor:C} - Conta: {ContaIdConta} - Centro: {CentroCustoId}";
    }
}
