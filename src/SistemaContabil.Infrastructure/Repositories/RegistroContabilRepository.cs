using Microsoft.EntityFrameworkCore;
using SistemaContabil.Domain.Entities;
using SistemaContabil.Domain.Interfaces;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Infrastructure.Repositories;

public class RegistroContabilRepository : Repository<RegistroContabil>, IRegistroContabilRepository
{
    public RegistroContabilRepository(SistemaContabilDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<RegistroContabil>> GetByContaAsync(int contaId)
    {
        return await _dbSet
            .Include(rc => rc.Conta)
            .Include(rc => rc.CentroCusto)
            .Where(rc => rc.ContaId == contaId)
            .OrderByDescending(rc => rc.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<RegistroContabil>> GetByCentroCustoAsync(int centroCustoId)
    {
        return await _dbSet
            .Include(rc => rc.Conta)
            .Include(rc => rc.CentroCusto)
            .Where(rc => rc.CentroCustoId == centroCustoId)
            .OrderByDescending(rc => rc.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<RegistroContabil>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _dbSet
            .Include(rc => rc.Conta)
            .Include(rc => rc.CentroCusto)
            .Where(rc => rc.DataCriacao >= dataInicio && rc.DataCriacao <= dataFim)
            .OrderByDescending(rc => rc.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<RegistroContabil>> GetByValorAsync(decimal valorMinimo, decimal valorMaximo)
    {
        return await _dbSet
            .Include(rc => rc.Conta)
            .Include(rc => rc.CentroCusto)
            .Where(rc => rc.Valor >= valorMinimo && rc.Valor <= valorMaximo)
            .OrderByDescending(rc => rc.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<RegistroContabil>> GetWithDetailsAsync()
    {
        return await _dbSet
            .Include(rc => rc.Conta)
            .Include(rc => rc.CentroCusto)
            .OrderByDescending(rc => rc.DataCriacao)
            .ToListAsync();
    }

    public async Task<RegistroContabil?> GetWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(rc => rc.Conta)
            .Include(rc => rc.CentroCusto)
            .FirstOrDefaultAsync(rc => rc.IdRegCont == id);
    }

    public async Task<decimal> GetTotalByContaAsync(int contaId)
    {
        return await _dbSet
            .Where(rc => rc.ContaId == contaId)
            .SumAsync(rc => rc.Valor);
    }

    public async Task<decimal> GetTotalByCentroCustoAsync(int centroCustoId)
    {
        return await _dbSet
            .Where(rc => rc.CentroCustoId == centroCustoId)
            .SumAsync(rc => rc.Valor);
    }

    public async Task<decimal> GetTotalByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _dbSet
            .Where(rc => rc.DataCriacao >= dataInicio && rc.DataCriacao <= dataFim)
            .SumAsync(rc => rc.Valor);
    }

    public async Task<IEnumerable<RegistroContabil>> GetOrderedByDataAsync(bool ascending = true)
    {
        var query = _dbSet
            .Include(rc => rc.Conta)
            .Include(rc => rc.CentroCusto)
            .AsQueryable();

        return ascending 
            ? await query.OrderBy(rc => rc.DataCriacao).ToListAsync()
            : await query.OrderByDescending(rc => rc.DataCriacao).ToListAsync();
    }

    public async Task<IEnumerable<RegistroContabil>> GetOrderedByValorAsync(bool ascending = true)
    {
        var query = _dbSet
            .Include(rc => rc.Conta)
            .Include(rc => rc.CentroCusto)
            .AsQueryable();

        return ascending 
            ? await query.OrderBy(rc => rc.Valor).ToListAsync()
            : await query.OrderByDescending(rc => rc.Valor).ToListAsync();
    }

    // Métodos da interface IRegistroContabilRepository
    public async Task<IEnumerable<RegistroContabil>> ObterPorContaAsync(int contaId)
    {
        return await GetByContaAsync(contaId);
    }

    public async Task<IEnumerable<RegistroContabil>> ObterPorCentroCustoAsync(int centroCustoId)
    {
        return await GetByCentroCustoAsync(centroCustoId);
    }

    public async Task<IEnumerable<RegistroContabil>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await GetByPeriodoAsync(dataInicio, dataFim);
    }

    public async Task<IEnumerable<RegistroContabil>> ObterPorValorAsync(decimal valorMinimo, decimal valorMaximo)
    {
        return await GetByValorAsync(valorMinimo, valorMaximo);
    }

    public async Task<IEnumerable<RegistroContabil>> ObterComDetalhesAsync()
    {
        return await GetWithDetailsAsync();
    }

    public async Task<RegistroContabil?> ObterComDetalhesAsync(int id)
    {
        return await GetWithDetailsAsync(id);
    }

    public async Task<decimal> CalcularTotalPorContaAsync(int contaId)
    {
        return await GetTotalByContaAsync(contaId);
    }

    public async Task<decimal> CalcularTotalPorCentroCustoAsync(int centroCustoId)
    {
        return await GetTotalByCentroCustoAsync(centroCustoId);
    }

    public async Task<decimal> CalcularTotalPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await GetTotalByPeriodoAsync(dataInicio, dataFim);
    }

    // Implementação dos métodos da interface para compatibilidade com serviços de domínio
    public async Task<RegistroContabil?> ObterPorIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<IEnumerable<RegistroContabil>> ObterTodosAsync()
    {
        return await GetAllAsync();
    }

    public async Task<RegistroContabil> AdicionarAsync(RegistroContabil entity)
    {
        // Gerar ID usando a sequência antes de inserir
        if (entity.IdRegCont == 0)
        {
            entity.IdRegCont = await GetNextSequenceValueAsync("reg_cont_seq");
        }
        return await AddAsync(entity);
    }

    public async Task<RegistroContabil> AtualizarAsync(RegistroContabil entity)
    {
        return await UpdateAsync(entity);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await RemoveByIdAsync(id);
    }

    public async Task<(IEnumerable<RegistroContabil> Items, int TotalCount)> SearchPagedAsync(
        decimal? valorMin = null,
        decimal? valorMax = null,
        int? contaId = null,
        int? centroCustoId = null,
        DateTime? dataInicio = null,
        DateTime? dataFim = null,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool isDescending = false)
    {
        var query = _dbSet
            .Include(rc => rc.Conta)
            .Include(rc => rc.CentroCusto)
            .AsQueryable();

        // Aplicar filtros
        if (valorMin.HasValue)
        {
            query = query.Where(rc => rc.Valor >= valorMin.Value);
        }

        if (valorMax.HasValue)
        {
            query = query.Where(rc => rc.Valor <= valorMax.Value);
        }

        if (contaId.HasValue)
        {
            query = query.Where(rc => rc.ContaId == contaId.Value);
        }

        if (centroCustoId.HasValue)
        {
            query = query.Where(rc => rc.CentroCustoId == centroCustoId.Value);
        }

        if (dataInicio.HasValue)
        {
            query = query.Where(rc => rc.DataCriacao >= dataInicio.Value);
        }

        if (dataFim.HasValue)
        {
            query = query.Where(rc => rc.DataCriacao <= dataFim.Value);
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

    private IQueryable<RegistroContabil> ApplySorting(IQueryable<RegistroContabil> query, string? sortBy, bool isDescending)
    {
        return sortBy?.ToLowerInvariant() switch
        {
            "valor" => isDescending
                ? query.OrderByDescending(rc => rc.Valor)
                : query.OrderBy(rc => rc.Valor),
            "data" or "datacriacao" => isDescending
                ? query.OrderByDescending(rc => rc.DataCriacao)
                : query.OrderBy(rc => rc.DataCriacao),
            "contaid" => isDescending
                ? query.OrderByDescending(rc => rc.ContaId)
                : query.OrderBy(rc => rc.ContaId),
            "centrocustoid" => isDescending
                ? query.OrderByDescending(rc => rc.CentroCustoId)
                : query.OrderBy(rc => rc.CentroCustoId),
            "id" or "idregcont" => isDescending
                ? query.OrderByDescending(rc => rc.IdRegCont)
                : query.OrderBy(rc => rc.IdRegCont),
            _ => query.OrderByDescending(rc => rc.DataCriacao)
        };
    }
}
