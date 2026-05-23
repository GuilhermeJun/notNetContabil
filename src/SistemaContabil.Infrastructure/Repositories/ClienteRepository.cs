using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Infrastructure.Repositories;

public class ClienteRepository : Repository<Cliente>
{
    public ClienteRepository(SistemaContabilDb context) : base(context)
    {
    }

    public async Task<Cliente?> GetByCpfAsync(string cpf)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Cpf == cpf);
    }

    public async Task<Cliente?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<bool> ExistsByCpfCnpjAsync(string cpfCnpj, int? excludeId = null)
    {
        var query = _dbSet.Where(c => c.Cpf == cpfCnpj);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
    {
        var query = _dbSet.Where(c => c.Email == email);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Cliente>> SearchByNomeAsync(string nome)
    {
        return await _dbSet
            .Where(c => c.Nome.Contains(nome))
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<Cliente?> ObterPorIdAsync(int id) => await GetByIdAsync(id);
    public async Task<IEnumerable<Cliente>> ObterTodosAsync() => await GetAllAsync();
    
    public async Task<Cliente> AdicionarAsync(Cliente entity)
    {
        // Gerar ID usando a sequência antes de inserir
        if (entity.Id == 0)
        {
            entity.Id = await GetNextSequenceValueAsync("cliente_seq");
        }
        return await AddAsync(entity);
    }
    
    public async Task<Cliente> AtualizarAsync(Cliente entity) => await UpdateAsync(entity);
    public async Task<bool> RemoverAsync(int id) => await RemoveByIdAsync(id);
}
