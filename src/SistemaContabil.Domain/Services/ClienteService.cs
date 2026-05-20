using SistemaContabil.Domain.Entities;
using SistemaContabil.Domain.Interfaces;

namespace SistemaContabil.Domain.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;

    public ClienteService(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<Cliente> CriarAsync(string nome, string cpf, string email, char ativo = 'S')
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do cliente é obrigatório", nameof(nome));

        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF/CNPJ é obrigatório", nameof(cpf));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório", nameof(email));

        if (ativo is not ('S' or 'N'))
            throw new ArgumentException("Status ativo deve ser 'S' ou 'N'", nameof(ativo));

        if (await _repository.GetByCpfCnpjAsync(cpf) is not null)
            throw new ArgumentException("Já existe um cliente com este CPF/CNPJ", nameof(cpf));

        if (await _repository.GetByEmailAsync(email) is not null)
            throw new ArgumentException("Já existe um cliente com este email", nameof(email));

        var cliente = new Cliente
        {
            Nome = nome.Trim(),
            Cpf = cpf.Trim(),
            Email = email.Trim(),
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

        if (ativo is not ('S' or 'N'))
            throw new ArgumentException("Status ativo deve ser 'S' ou 'N'", nameof(ativo));

        var cliente = await _repository.ObterPorIdAsync(id);
        if (cliente is null)
            throw new InvalidOperationException($"Cliente com ID {id} não encontrado");

        var existenteEmail = await _repository.GetByEmailAsync(email);
        if (existenteEmail is not null && existenteEmail.Id != id)
            throw new ArgumentException("Já existe outro cliente com este email", nameof(email));

        cliente.Nome = nome.Trim();
        cliente.Email = email.Trim();
        cliente.AtualizarStatus(ativo == 'S');

        return await _repository.AtualizarAsync(cliente);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var cliente = await _repository.ObterPorIdAsync(id);
        if (cliente is null)
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

    public async Task<Cliente?> ObterPorCpfAsync(string cpf)
    {
        return await _repository.GetByCpfCnpjAsync(cpf);
    }

    public async Task<Cliente?> ObterPorEmailAsync(string email)
    {
        return await _repository.GetByEmailAsync(email);
    }
}
