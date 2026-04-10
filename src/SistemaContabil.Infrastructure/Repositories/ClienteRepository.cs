using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Infrastructure.Repositories;

public class ClienteRepository : Repository<Cliente>, IClienteRepository
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

    public async Task<IEnumerable<Cliente>> SearchByNomeAsync(string nome)
    {
        return await _dbSet
            .Where(c => c.NomeCliente.Contains(nome))
            .OrderBy(c => c.NomeCliente)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Cliente> Items, int TotalCount)> SearchPagedAsync(
        string? nome = null,
        string? cpfCnpj = null,
        char? ativo = null,
        string? email = null,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool isDescending = false)
    {
        var query = _dbSet.AsQueryable();

        // Aplicar filtros
        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(c => c.NomeCliente.Contains(nome));
        }

        if (!string.IsNullOrWhiteSpace(cpfCnpj))
        {
            query = query.Where(c => c.CpfCnpj == cpfCnpj);
        }

        if (ativo.HasValue)
        {
            query = query.Where(c => c.Ativo == ativo.Value);
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            query = query.Where(c => c.Email.Contains(email));
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

    private IQueryable<Cliente> ApplySorting(IQueryable<Cliente> query, string? sortBy, bool isDescending)
    {
        return sortBy?.ToLowerInvariant() switch
        {
            "nome" or "nomecliente" => isDescending 
                ? query.OrderByDescending(c => c.NomeCliente)
                : query.OrderBy(c => c.NomeCliente),
            "cpfcnpj" => isDescending
                ? query.OrderByDescending(c => c.CpfCnpj)
                : query.OrderBy(c => c.CpfCnpj),
            "email" => isDescending
                ? query.OrderByDescending(c => c.Email)
                : query.OrderBy(c => c.Email),
            "datacadastro" => isDescending
                ? query.OrderByDescending(c => c.DataCadastro)
                : query.OrderBy(c => c.DataCadastro),
            _ => query.OrderBy(c => c.NomeCliente)
        };
    }

    // Métodos de compatibilidade
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
