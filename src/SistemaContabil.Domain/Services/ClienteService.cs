using SistemaContabil.Domain.Entities;
using SistemaContabil.Domain.Interfaces;

namespace SistemaContabil.Domain.Services;

/// Serviço de domínio para Cliente
public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;

    public ClienteService(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<Cliente> CriarAsync(string nome, string cpfCnpj, string email, string senha, char ativo = 'S')
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do cliente é obrigatório", nameof(nome));

        if (string.IsNullOrWhiteSpace(cpfCnpj))
            throw new ArgumentException("CPF/CNPJ é obrigatório", nameof(cpfCnpj));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório", nameof(email));

        if (string.IsNullOrWhiteSpace(senha))
            throw new ArgumentException("Senha é obrigatória", nameof(senha));

        if (ativo != 'S' && ativo != 'N')
            throw new ArgumentException("Status ativo deve ser 'S' ou 'N'", nameof(ativo));

        // Verificar se já existe cliente com mesmo CPF/CNPJ
        var existenteCpf = await _repository.GetByCpfCnpjAsync(cpfCnpj);
        if (existenteCpf != null)
            throw new ArgumentException("Já existe um cliente com este CPF/CNPJ", nameof(cpfCnpj));

        // Verificar se já existe cliente com mesmo email
        var existenteEmail = await _repository.GetByEmailAsync(email);
        if (existenteEmail != null)
            throw new ArgumentException("Já existe um cliente com este email", nameof(email));

        var cliente = new Cliente
        {
            NomeCliente = nome.Trim(),
            CpfCnpj = cpfCnpj.Trim(),
            Email = email.Trim(),
            Senha = senha,
            Ativo = ativo,
            DataCadastro = DateTime.Now
        };

        return await _repository.AdicionarAsync(cliente);
    }

    public async Task<Cliente> AtualizarAsync(int id, string nome, string email, char ativo)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do cliente é obrigatório", nameof(nome));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório", nameof(email));

        if (ativo != 'S' && ativo != 'N')
            throw new ArgumentException("Status ativo deve ser 'S' ou 'N'", nameof(ativo));

        var cliente = await _repository.ObterPorIdAsync(id);
        if (cliente == null)
            throw new InvalidOperationException($"Cliente com ID {id} não encontrado");

        // Verificar se outro cliente já usa este email
        var existenteEmail = await _repository.GetByEmailAsync(email);
        if (existenteEmail != null && existenteEmail.IdCliente != id)
            throw new ArgumentException("Já existe outro cliente com este email", nameof(email));

        cliente.NomeCliente = nome.Trim();
        cliente.Email = email.Trim();
        cliente.AtualizarStatus(ativo == 'S');

        return await _repository.AtualizarAsync(cliente);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var cliente = await _repository.ObterPorIdAsync(id);
        if (cliente == null)
            throw new InvalidOperationException($"Cliente com ID {id} não encontrado");

        return await _repository.RemoverAsync(id);
    }

    public async Task<Cliente?> ObterPorIdAsync(int id)
    {
        return await _repository.ObterPorIdAsync(id);
    }

    public async Task<IEnumerable<Cliente>> ObterTodosAsync()
    {
        return await _repository.ObterTodosAsync();
    }

    public async Task<IEnumerable<Cliente>> BuscarPorNomeAsync(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Enumerable.Empty<Cliente>();

        return await _repository.SearchByNomeAsync(nome);
    }

    public async Task<Cliente?> ObterPorCpfCnpjAsync(string cpfCnpj)
    {
        return await _repository.GetByCpfCnpjAsync(cpfCnpj);
    }

    public async Task<Cliente?> ObterPorEmailAsync(string email)
    {
        return await _repository.GetByEmailAsync(email);
    }
}
