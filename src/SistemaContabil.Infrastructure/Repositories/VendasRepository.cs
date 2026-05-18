using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Infrastructure.Repositories;

public class VendasRepository : Repository<Venda>, IVendasRepository
{
    public VendasRepository(SistemaContabilDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Venda>> GetByClienteIdAsync(int clienteId)
    {
        return await _dbSet
            .Include(v => v.Cliente)
            .Include(v => v.RegistroContabil)
            .Where(v => v.ClienteId == clienteId)
            .OrderByDescending(v => v.IdVendas)
            .ToListAsync();
    }

    public async Task<IEnumerable<Venda>> GetByRegContIdAsync(int regContId)
    {
        return await _dbSet
            .Include(v => v.Cliente)
            .Include(v => v.RegistroContabil)
            .Where(v => v.RegContId == regContId)
            .OrderByDescending(v => v.IdVendas)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Venda> Items, int TotalCount)> SearchPagedAsync(
        int? clienteId = null,
        int? regContId = null,
        long? vendaEventoId = null,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool isDescending = false)
    {
        var query = _dbSet
            .Include(v => v.Cliente)
            .Include(v => v.RegistroContabil)
            .AsQueryable();

        // Aplicar filtros
        if (clienteId.HasValue)
        {
            query = query.Where(v => v.ClienteId == clienteId.Value);
        }

        if (regContId.HasValue)
        {
            query = query.Where(v => v.RegContId == regContId.Value);
        }

        if (vendaEventoId.HasValue)
        {
            query = query.Where(v => v.VendaEventoId == vendaEventoId.Value);
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

    public async Task<IEnumerable<Venda>> GetByClienteAsync(int clienteId)
    {
        return await GetByClienteIdAsync(clienteId);
    }

    public async Task<IEnumerable<Venda>> GetByRegContAsync(int regContId)
    {
        return await GetByRegContIdAsync(regContId);
    }

    private IQueryable<Venda> ApplySorting(IQueryable<Venda> query, string? sortBy, bool isDescending)
    {
        return sortBy?.ToLowerInvariant() switch
        {
            "id" or "idvendas" => isDescending
                ? query.OrderByDescending(v => v.IdVendas)
                : query.OrderBy(v => v.IdVendas),
            "clienteid" or "cliente" => isDescending
                ? query.OrderByDescending(v => v.ClienteId)
                : query.OrderBy(v => v.ClienteId),
            "regcontid" => isDescending
                ? query.OrderByDescending(v => v.RegContId)
                : query.OrderBy(v => v.RegContId),
            _ => query.OrderByDescending(v => v.IdVendas)
        };
    }

    // Métodos de compatibilidade
    public async Task<Venda?> ObterPorIdAsync(int id) => await GetByIdAsync(id);
    public async Task<IEnumerable<Venda>> ObterTodosAsync() => await GetAllAsync();
    
    public async Task<Venda> AdicionarAsync(Venda entity)
    {
        // Gerar ID usando a sequência antes de inserir
        if (entity.IdVendas == 0)
        {
            entity.IdVendas = await GetNextSequenceValueAsync("vendas_seq");
        }
        return await AddAsync(entity);
    }
    
    public async Task<Venda> AtualizarAsync(Venda entity) => await UpdateAsync(entity);
    public async Task<bool> RemoverAsync(int id) => await RemoveByIdAsync(id);
}
