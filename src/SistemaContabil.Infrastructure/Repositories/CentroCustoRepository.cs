using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Infrastructure.Repositories;

public class CentroCustoRepository : Repository<CentroCusto>, ICentroCustoRepository
{
    public CentroCustoRepository(SistemaContabilDbContext context) : base(context)
    {
    }

    public async Task<CentroCusto?> GetByNomeAsync(string nome)
    {
        return await _dbSet
            .FirstOrDefaultAsync(cc => cc.NomeCentroCusto == nome);
    }

    public async Task<bool> ExistsByNomeAsync(string nome, int? excludeId = null)
    {
        var query = _dbSet.Where(cc => cc.NomeCentroCusto == nome);
        
        if (excludeId.HasValue)
        {
            query = query.Where(cc => cc.IdCentroCusto != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<CentroCusto>> SearchByNomeAsync(string texto)
    {
        return await _dbSet
            .Where(cc => cc.NomeCentroCusto.Contains(texto))
            .OrderBy(cc => cc.NomeCentroCusto)
            .ToListAsync();
    }

    public async Task<IEnumerable<CentroCusto>> BuscarPorNomeAsync(string nome)
    {
        return await SearchByNomeAsync(nome);
    }

    public async Task<IEnumerable<CentroCusto>> GetWithRegistrosAsync()
    {
        return await _dbSet
            .Include(cc => cc.RegistrosContabeis)
            .Where(cc => cc.RegistrosContabeis.Any())
            .OrderBy(cc => cc.NomeCentroCusto)
            .ToListAsync();
    }

    public async Task<CentroCusto?> GetWithRegistrosAsync(int id)
    {
        return await _dbSet
            .Include(cc => cc.RegistrosContabeis)
            .ThenInclude(rc => rc.Conta)
            .FirstOrDefaultAsync(cc => cc.IdCentroCusto == id);
    }

    // Implementação dos métodos da interface para compatibilidade com serviços de domínio
    public async Task<CentroCusto?> ObterPorIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<IEnumerable<CentroCusto>> ObterTodosAsync()
    {
        return await GetAllAsync();
    }

    public async Task<CentroCusto> AdicionarAsync(CentroCusto entity)
    {
        // Gerar ID usando a sequência antes de inserir
        if (entity.IdCentroCusto == 0)
        {
            entity.IdCentroCusto = await GetNextSequenceValueAsync("centro_custo_seq");
        }
        return await AddAsync(entity);
    }

    public async Task<CentroCusto> AtualizarAsync(CentroCusto entity)
    {
        return await UpdateAsync(entity);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await RemoveByIdAsync(id);
    }

    public async Task<(IEnumerable<CentroCusto> Items, int TotalCount)> SearchPagedAsync(
        string? nome = null,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool isDescending = false)
    {
        var query = _dbSet.AsQueryable();

        // Aplicar filtros
        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(cc => cc.NomeCentroCusto.Contains(nome));
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

    private IQueryable<CentroCusto> ApplySorting(IQueryable<CentroCusto> query, string? sortBy, bool isDescending)
    {
        return sortBy?.ToLowerInvariant() switch
        {
            "nome" or "nomecentrocusto" => isDescending
                ? query.OrderByDescending(cc => cc.NomeCentroCusto)
                : query.OrderBy(cc => cc.NomeCentroCusto),
            "id" or "idcentrocusto" => isDescending
                ? query.OrderByDescending(cc => cc.IdCentroCusto)
                : query.OrderBy(cc => cc.IdCentroCusto),
            _ => query.OrderBy(cc => cc.NomeCentroCusto)
        };
    }
}
