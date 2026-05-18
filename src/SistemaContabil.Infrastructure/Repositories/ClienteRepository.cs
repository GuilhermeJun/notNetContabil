using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Infrastructure.Repositories;

public class ClienteRepository : Repository<Cliente>
{
    public ClienteRepository(SistemaContabilDbContext context) : base(context)
    {
    }

    public async Task<Cliente?> GetByCpfCnpjAsync(string cpfCnpj)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.CpfCnpj == cpfCnpj);
    }

    public async Task<Cliente?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<bool> ExistsByCpfCnpjAsync(string cpfCnpj, int? excludeId = null)
    {
        var query = _dbSet.Where(c => c.CpfCnpj == cpfCnpj);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.IdCliente != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
    {
        var query = _dbSet.Where(c => c.Email == email);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.IdCliente != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<Cliente?> ObterPorIdAsync(int id) => await GetByIdAsync(id);
    public async Task<IEnumerable<Cliente>> ObterTodosAsync() => await GetAllAsync();
    
    public async Task<Cliente> AdicionarAsync(Cliente entity)
    {
        // Gerar ID usando a sequência antes de inserir
        if (entity.IdCliente == 0)
        {
            entity.IdCliente = await GetNextSequenceValueAsync("cliente_seq");
        }
        return await AddAsync(entity);
    }
    
    public async Task<Cliente> AtualizarAsync(Cliente entity) => await UpdateAsync(entity);
    public async Task<bool> RemoverAsync(int id) => await RemoveByIdAsync(id);
}
