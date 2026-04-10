using System.ComponentModel.DataAnnotations;

namespace SistemaContabil.Domain.Entities;

public class CentroCusto
{
    [Key]
    public int IdCentroCusto { get; set; }

    [Required]
    [StringLength(70)]
    public string NomeCentroCusto { get; set; } = string.Empty;

    public virtual ICollection<RegistroContabil> RegistrosContabeis { get; set; } = new List<RegistroContabil>();

    /// Valida se o centro de custo está válido para operações
    /// <returns>True se válido, False caso contrário</returns>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(NomeCentroCusto) && 
               NomeCentroCusto.Length <= 70;
    }

    /// Atualiza o nome do centro de custo
    /// <param name="novoNome">Novo nome para o centro de custo</param>
    /// <exception cref="ArgumentException">Lançada quando o nome é inválido</exception>
    public void AtualizarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new ArgumentException("Nome do centro de custo não pode ser vazio", nameof(novoNome));
        
        if (novoNome.Length > 70)
            throw new ArgumentException("Nome do centro de custo não pode ter mais de 70 caracteres", nameof(novoNome));

        NomeCentroCusto = novoNome.Trim();
    }
}
