using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Infrastructure.Data;

public class SistemaContabilDbContext : DbContext
{
    public SistemaContabilDbContext(DbContextOptions<SistemaContabilDbContext> options) : base(options)
    {
    }
    public DbSet<CentroCusto> CentrosCusto { get; set; } = null!;

    public DbSet<Conta> Contas { get; set; } = null!;

    public DbSet<RegistroContabil> RegistrosContabeis { get; set; } = null!;

    public DbSet<Cliente> Clientes { get; set; } = null!;

    public DbSet<Vendas> Vendas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da tabela CentroCusto
        modelBuilder.Entity<CentroCusto>(entity =>
        {
            entity.ToTable("CENTRO_CUSTO");
            entity.HasKey(e => e.IdCentroCusto);
            entity.Property(e => e.IdCentroCusto)
                .HasColumnName("ID_CENTRO_CUSTO")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.NomeCentroCusto)
                .HasColumnName("NOME_CENTRO_CUSTO")
                .HasColumnType("VARCHAR2(70)")
                .IsRequired();

            // Relacionamento com registros contábeis
            entity.HasMany(e => e.RegistrosContabeis)
                .WithOne(e => e.CentroCusto)
                .HasForeignKey(e => e.CentroCustoIdCentroCusto)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração da tabela Conta Contábil
        modelBuilder.Entity<Conta>(entity =>
        {
            entity.ToTable("CONTA_CONTABIL");
            entity.HasKey(e => e.IdContaContabil);
            entity.Property(e => e.IdContaContabil)
                .HasColumnName("ID_CONTA_CONTABIL")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.NomeContaContabil)
                .HasColumnName("NOME_CONTA_CONTABIL")
                .HasColumnType("VARCHAR2(70)")
                .IsRequired();
            entity.Property(e => e.Tipo)
                .HasColumnName("TIPO")
                .HasColumnType("CHAR(1)")
                .HasConversion(
                    v => v.ToString(),
                    v => v.Length > 0 ? v[0] : 'D')
                .IsRequired();
            entity.Property(e => e.ClienteIdCliente)
                .HasColumnName("CLIENTE_ID_CLIENTE");

            // Relacionamento com cliente (opcional)
            entity.HasOne(e => e.Cliente)
                .WithMany(c => c.Contas)
                .HasForeignKey(e => e.ClienteIdCliente)
                .OnDelete(DeleteBehavior.SetNull);

            // Relacionamento com registros contábeis
            entity.HasMany(e => e.RegistrosContabeis)
                .WithOne(e => e.Conta)
                .HasForeignKey(e => e.ContaIdConta)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração da tabela Registro Contábil (REG_CONT)
        modelBuilder.Entity<RegistroContabil>(entity =>
        {
            entity.ToTable("REG_CONT");
            entity.HasKey(e => e.IdRegCont);
            entity.Property(e => e.IdRegCont)
                .HasColumnName("ID_REG_CONT")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Valor)
                .HasColumnName("VALOR")
                .HasColumnType("NUMBER(9,2)")
                .IsRequired();
            entity.Property(e => e.ContaIdConta)
                .HasColumnName("CONTA_CONTABIL_ID_CONTA")
                .IsRequired();
            entity.Property(e => e.CentroCustoIdCentroCusto)
                .HasColumnName("CENTRO_CUSTO_ID_CENTRO_CUSTO")
                .IsRequired();
            entity.Property(e => e.DataCriacao)
                .HasColumnName("DATA_CRIACAO")
                .HasColumnType("DATE")
                .HasDefaultValueSql("SYSDATE");
            entity.Property(e => e.DataAtualizacao)
                .HasColumnName("DATA_ATUALIZACAO")
                .HasColumnType("DATE");

            // Relacionamentos
            entity.HasOne(e => e.Conta)
                .WithMany(e => e.RegistrosContabeis)
                .HasForeignKey(e => e.ContaIdConta)
                .HasPrincipalKey(c => c.IdContaContabil)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CentroCusto)
                .WithMany(e => e.RegistrosContabeis)
                .HasForeignKey(e => e.CentroCustoIdCentroCusto)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração da tabela Cliente
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("CLIENTE");
            entity.HasKey(e => e.IdCliente);
            entity.Property(e => e.IdCliente)
                .HasColumnName("ID_CLIENTE")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.NomeCliente)
                .HasColumnName("NOME_CLIENTE")
                .HasColumnType("VARCHAR2(100)")
                .IsRequired();
            entity.Property(e => e.DataCadastro)
                .HasColumnName("DATA_CADASTRO")
                .HasColumnType("DATE")
                .HasDefaultValueSql("SYSDATE");
            entity.Property(e => e.CpfCnpj)
                .HasColumnName("CPF_CNPJ")
                .HasColumnType("VARCHAR2(14)")
                .IsRequired();
            entity.Property(e => e.Email)
                .HasColumnName("EMAIL")
                .HasColumnType("VARCHAR2(100)")
                .IsRequired();
            entity.Property(e => e.Senha)
                .HasColumnName("SENHA")
                .HasColumnType("VARCHAR2(100)")
                .IsRequired();
            entity.Property(e => e.Ativo)
                .HasColumnName("ATIVO")
                .HasColumnType("CHAR(1)")
                .HasConversion(
                    v => v.ToString(),
                    v => v.Length > 0 ? v[0] : 'S')
                .HasDefaultValue('S')
                .IsRequired();

            // Índices únicos
            entity.HasIndex(e => e.CpfCnpj)
                .IsUnique()
                .HasDatabaseName("CLIENTE_CPF_CNPJ_UN");
            entity.HasIndex(e => e.Email)
                .IsUnique()
                .HasDatabaseName("CLIENTE_EMAIL_UN");

            // Relacionamentos
            entity.HasMany(e => e.Contas)
                .WithOne(c => c.Cliente)
                .HasForeignKey(c => c.ClienteIdCliente)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(e => e.Vendas)
                .WithOne(v => v.Cliente)
                .HasForeignKey(v => v.ClienteIdCliente)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração da tabela Vendas
        modelBuilder.Entity<Vendas>(entity =>
        {
            entity.ToTable("VENDAS");
            entity.HasKey(e => e.IdVendas);
            entity.Property(e => e.IdVendas)
                .HasColumnName("ID_VENDAS")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.ClienteIdCliente)
                .HasColumnName("CLIENTE_ID_CLIENTE")
                .IsRequired();
            entity.Property(e => e.RegContIdRegCont)
                .HasColumnName("REG_CONT_ID_REG_CONT")
                .IsRequired();
            entity.Property(e => e.VendaEventoIdEvento)
                .HasColumnName("VENDA_EVENTO_ID_EVENTO");

            // Índices únicos
            entity.HasIndex(e => e.VendaEventoIdEvento)
                .IsUnique()
                .HasDatabaseName("VENDA_ID_EVENTO_UN");

            // Relacionamentos
            entity.HasOne(e => e.Cliente)
                .WithMany(c => c.Vendas)
                .HasForeignKey(e => e.ClienteIdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.RegistroContabil)
                .WithMany(r => r.Vendas)
                .HasForeignKey(e => e.RegContIdRegCont)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração de índices para performance
        modelBuilder.Entity<RegistroContabil>()
            .HasIndex(e => e.ContaIdConta)
            .HasDatabaseName("IX_REG_CONT_CONTA");

        modelBuilder.Entity<RegistroContabil>()
            .HasIndex(e => e.CentroCustoIdCentroCusto)
            .HasDatabaseName("IX_REG_CONT_CCUSTO");

        modelBuilder.Entity<Conta>()
            .HasIndex(e => e.ClienteIdCliente)
            .HasDatabaseName("IX_CONTA_CLIENTE");

        modelBuilder.Entity<Vendas>()
            .HasIndex(e => e.ClienteIdCliente)
            .HasDatabaseName("IX_VENDAS_CLIENTE");

        modelBuilder.Entity<Vendas>()
            .HasIndex(e => e.RegContIdRegCont)
            .HasDatabaseName("IX_VENDAS_REG_CONT");

        // Configuração de sequências Oracle (conforme SQL)
        modelBuilder.HasSequence<int>("centro_custo_seq")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.HasSequence<int>("cliente_seq")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.HasSequence<int>("conta_seq")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.HasSequence<int>("reg_cont_seq")
            .StartsAt(1)
            .IncrementsBy(1);

        modelBuilder.HasSequence<long>("vendas_seq")
            .StartsAt(1)
            .IncrementsBy(1);
    }
}
