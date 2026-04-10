using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Infrastructure.Repositories;

public class VendasRepository : Repository<Vendas>, IVendasRepository
{
    public VendasRepository(SistemaContabilDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Vendas>> GetByClienteIdAsync(int clienteId)
    {
        return await _dbSet
            .Include(v => v.Cliente)
            .Include(v => v.RegistroContabil)
            .Where(v => v.ClienteIdCliente == clienteId)
            .OrderByDescending(v => v.IdVendas)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vendas>> GetByRegContIdAsync(int regContId)
    {
        return await _dbSet
            .Include(v => v.Cliente)
            .Include(v => v.RegistroContabil)
            .Where(v => v.RegContIdRegCont == regContId)
            .OrderByDescending(v => v.IdVendas)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Vendas> Items, int TotalCount)> SearchPagedAsync(
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
            query = query.Where(v => v.ClienteIdCliente == clienteId.Value);
        }

        if (regContId.HasValue)
        {
            query = query.Where(v => v.RegContIdRegCont == regContId.Value);
        }

        if (vendaEventoId.HasValue)
        {
            query = query.Where(v => v.VendaEventoIdEvento == vendaEventoId.Value);
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

    public async Task<IEnumerable<Vendas>> GetByClienteAsync(int clienteId)
    {
        return await GetByClienteIdAsync(clienteId);
    }

    public async Task<IEnumerable<Vendas>> GetByRegContAsync(int regContId)
    {
        return await GetByRegContIdAsync(regContId);
    }

    private IQueryable<Vendas> ApplySorting(IQueryable<Vendas> query, string? sortBy, bool isDescending)
    {
        return sortBy?.ToLowerInvariant() switch
        {
            "id" or "idvendas" => isDescending
                ? query.OrderByDescending(v => v.IdVendas)
                : query.OrderBy(v => v.IdVendas),
            "clienteid" or "cliente" => isDescending
                ? query.OrderByDescending(v => v.ClienteIdCliente)
                : query.OrderBy(v => v.ClienteIdCliente),
            "regcontid" => isDescending
                ? query.OrderByDescending(v => v.RegContIdRegCont)
                : query.OrderBy(v => v.RegContIdRegCont),
            _ => query.OrderByDescending(v => v.IdVendas)
        };
    }

    // Métodos de compatibilidade
    public async Task<Vendas?> ObterPorIdAsync(int id) => await GetByIdAsync(id);
    public async Task<IEnumerable<Vendas>> ObterTodosAsync() => await GetAllAsync();
    
    public async Task<Vendas> AdicionarAsync(Vendas entity)
    {
        // Gerar ID usando a sequência antes de inserir
        if (entity.IdVendas == 0)
        {
            entity.IdVendas = await GetNextSequenceValueAsync("vendas_seq");
        }
        return await AddAsync(entity);
    }
    
    public async Task<Vendas> AtualizarAsync(Vendas entity) => await UpdateAsync(entity);
    public async Task<bool> RemoverAsync(int id) => await RemoveByIdAsync(id);
}
