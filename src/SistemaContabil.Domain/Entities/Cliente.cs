using System.ComponentModel.DataAnnotations;

namespace SistemaContabil.Domain.Entities;

public class Cliente
{
    [Key]
    public int IdCliente { get; set; }

    [Required]
    [StringLength(100)]
    public string NomeCliente { get; set; } = string.Empty;

    [Required]
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    [Required]
    [StringLength(14)]
    public string CpfCnpj { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Senha { get; set; } = string.Empty;

    [Required]
    [StringLength(1)]
    public char Ativo { get; set; } = 'S';

    public virtual ICollection<Conta> Contas { get; set; } = new List<Conta>();

    public virtual ICollection<Vendas> Vendas { get; set; } = new List<Vendas>();

    /// Valida se o cliente está válido para operações
    /// <returns>True se válido, False caso contrário</returns>
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(NomeCliente) &&
               NomeCliente.Length <= 100 &&
               !string.IsNullOrWhiteSpace(CpfCnpj) &&
               CpfCnpj.Length <= 14 &&
               !string.IsNullOrWhiteSpace(Email) &&
               Email.Length <= 100 &&
               (Ativo == 'S' || Ativo == 'N');
    }

    /// Atualiza o status ativo/inativo do cliente
    /// <param name="ativo">True para ativar, False para desativar</param>
    public void AtualizarStatus(bool ativo)
    {
        Ativo = ativo ? 'S' : 'N';
    }
}
