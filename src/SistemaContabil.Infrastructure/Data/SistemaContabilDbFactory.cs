using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemaContabil.Infrastructure.Data;

public class SistemaContabilDbFactory
    : IDesignTimeDbContextFactory<SistemaContabilDb>
{
    public SistemaContabilDb CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SistemaContabilDb>();

        optionsBuilder.UseOracle(
            "Data Source=//oracle.fiap.com.br:1521/orcl;User Id=rm559986; Password=240200;");

        return new SistemaContabilDb(optionsBuilder.Options);
    }
}