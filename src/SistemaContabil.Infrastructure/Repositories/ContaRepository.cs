using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Infrastructure.Repositories;

public class ContaRepository : Repository<Conta>, IContaRepository
{
    public ContaRepository(SistemaContabilDbContext context) : base(context)
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

    public async Task<IEnumerable<Conta>> SearchByNomeAsync(string texto)
    {
        return await _dbSet
            .Where(c => c.NomeContaContabil.Contains(texto))
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

    public async Task<Conta?> GetWithRegistrosAsync(int id)
    {
        return await _dbSet
            .Include(c => c.RegistrosContabeis)
            .ThenInclude(rc => rc.CentroCusto)
            .FirstOrDefaultAsync(c => c.IdContaContabil == id);
    }

    public async Task<IEnumerable<Conta>> GetContasReceitaAsync()
    {
        return await GetByTipoAsync('R');
    }

    public async Task<IEnumerable<Conta>> GetContasDespesaAsync()
    {
        return await GetByTipoAsync('D');
    }

    // Métodos da interface IContaRepository
    public async Task<IEnumerable<Conta>> BuscarPorNomeAsync(string nome)
    {
        return await SearchByNomeAsync(nome);
    }

    public async Task<IEnumerable<Conta>> ObterPorTipoAsync(char tipo)
    {
        return await GetByTipoAsync(tipo);
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

    // Implementação dos métodos da interface para compatibilidade com serviços de domínio
    public async Task<Conta?> ObterPorIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<IEnumerable<Conta>> ObterTodosAsync()
    {
        return await GetAllAsync();
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

    public async Task<(IEnumerable<Conta> Items, int TotalCount)> SearchPagedAsync(
        string? nome = null,
        char? tipo = null,
        int? clienteId = null,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool isDescending = false)
    {
        var query = _dbSet.AsQueryable();

        // Aplicar filtros
        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(c => c.NomeContaContabil.Contains(nome));
        }

        if (tipo.HasValue)
        {
            query = query.Where(c => c.Tipo == tipo.Value);
        }

        if (clienteId.HasValue)
        {
            query = query.Where(c => c.ClienteIdCliente == clienteId.Value);
        }

        // Contar total
        var totalCount = await query.CountAsync();

        // Aplicar ordenação
        query = ApplySorting(query, sortBy, isDescending);

        // Aplicar paginação
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    private IQueryable<Conta> ApplySorting(IQueryable<Conta> query, string? sortBy, bool isDescending)
    {
        return sortBy?.ToLowerInvariant() switch
        {
            "nome" or "nomecontacontabil" => isDescending
                ? query.OrderByDescending(c => c.NomeContaContabil)
                : query.OrderBy(c => c.NomeContaContabil),
            "tipo" => isDescending
                ? query.OrderByDescending(c => c.Tipo)
                : query.OrderBy(c => c.Tipo),
            "id" or "idcontacontabil" => isDescending
                ? query.OrderByDescending(c => c.IdContaContabil)
                : query.OrderBy(c => c.IdContaContabil),
            _ => query.OrderBy(c => c.NomeContaContabil)
        };
    }
}
