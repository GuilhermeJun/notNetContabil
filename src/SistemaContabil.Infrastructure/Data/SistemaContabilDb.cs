using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Infrastructure.Data;

public class SistemaContabilDb : DbContext
{
    public SistemaContabilDb(DbContextOptions<SistemaContabilDb> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Conta> Contas { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<ItemVenda> ItensVenda { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }
    public DbSet<RegistroContabil> RegistrosContabeis { get; set; }

}
