using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Infrastructure.Repositories;

public class ContaRepository : Repository<Conta>
{
    public ContaRepository(SistemaContabilDb context) : base(context)
    {
    }

    public async Task<Conta?> GetByNomeAsync(string nome)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.NomeContaContabil == nome);
    }

    public async Task<bool> ExistsByNomeAsync(string nome, int? excludeId = null)
    {
        var query = _dbSet.Where(c => c.NomeContaContabil == nome);
        
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.IdContaContabil != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Conta>> GetByTipoAsync(char tipo)
    {
        return await _dbSet
            .Where(c => c.Tipo == tipo)
            .OrderBy(c => c.NomeContaContabil)
            .ToListAsync();
    }

    public async Task<IEnumerable<Conta>> GetWithRegistrosAsync()
    {
        return await _dbSet
            .Include(c => c.RegistrosContabeis)
            .Where(c => c.RegistrosContabeis.Any())
            .OrderBy(c => c.NomeContaContabil)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Conta>> GetContasReceitaAsync()
    {
        return await GetByTipoAsync('R');
    }

    public async Task<IEnumerable<Conta>> GetContasDespesaAsync()
    {
        return await GetByTipoAsync('D');
    }

    public async Task<IEnumerable<Conta>> ObterContasReceitaAsync()
    {
        return await GetContasReceitaAsync();
    }

    public async Task<IEnumerable<Conta>> ObterContasDespesaAsync()
    {
        return await GetContasDespesaAsync();
    }

    public async Task<IEnumerable<Conta>> ObterComRegistrosAsync()
    {
        return await GetWithRegistrosAsync();
    }

    public async Task<Conta> AdicionarAsync(Conta entity)
    {
        // Gerar ID usando a sequência antes de inserir
        if (entity.IdContaContabil == 0)
        {
            entity.IdContaContabil = await GetNextSequenceValueAsync("conta_seq");
        }
        return await AddAsync(entity);
    }

    public async Task<Conta> AtualizarAsync(Conta entity)
    {
        return await UpdateAsync(entity);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await RemoveByIdAsync(id);
    }

}
