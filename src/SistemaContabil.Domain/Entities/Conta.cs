using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaContabil.Domain.Entities;

public class Conta
{
    [Key]
    public int IdContaContabil { get; set; }

    [Required]
    [StringLength(70)]
    public string NomeContaContabil { get; set; } = string.Empty;

    [Required]
    [StringLength(1)]
    public char Tipo { get; set; }

    public int? ClienteIdCliente { get; set; }

    [ForeignKey(nameof(ClienteIdCliente))]
    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<RegistroContabil> RegistrosContabeis { get; set; } = new List<RegistroContabil>();

    /// Valida se a conta está válida para operações
    /// <returns>True se válido, False caso contrário</returns>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(NomeContaContabil) && 
               NomeContaContabil.Length <= 70 &&
               (Tipo == 'R' || Tipo == 'D');
    }

    /// Atualiza o nome da conta contábil
    /// <param name="novoNome">Novo nome para a conta</param>
    /// <exception cref="ArgumentException">Lançada quando o nome é inválido</exception>
    public void AtualizarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new ArgumentException("Nome da conta contábil não pode ser vazio", nameof(novoNome));
        
        if (novoNome.Length > 70)
            throw new ArgumentException("Nome da conta contábil não pode ter mais de 70 caracteres", nameof(novoNome));

        NomeContaContabil = novoNome.Trim();
    }

    /// Atualiza o tipo da conta
    /// <param name="novoTipo">Novo tipo para a conta (R ou D)</param>
    /// <exception cref="ArgumentException">Lançada quando o tipo é inválido</exception>
    public void AtualizarTipo(char novoTipo)
    {
        if (novoTipo != 'R' && novoTipo != 'D')
            throw new ArgumentException("Tipo da conta deve ser 'R' (Receita) ou 'D' (Despesa)", nameof(novoTipo));

        Tipo = novoTipo;
    }

    /// Retorna o tipo da conta como string descritiva
    /// <returns>Descrição do tipo da conta</returns>
    public string GetTipoDescricao()
    {
        return Tipo == 'R' ? "Receita" : "Despesa";
    }
}
