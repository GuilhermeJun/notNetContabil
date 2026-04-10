using AutoMapper;
using SistemaContabil.Application.DTOs;
using SistemaContabil.Domain.Entities;

namespace SistemaContabil.Application.Mappings;

/// Perfil de mapeamento do AutoMapper
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeamentos para CentroCusto
        CreateMap<CentroCusto, CentroCustoDto>()
            .ForMember(dest => dest.QuantidadeRegistros, opt => opt.MapFrom(src => src.RegistrosContabeis.Count))
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore());

        CreateMap<CentroCusto, CentroCustoDetalhesDto>()
            .ForMember(dest => dest.RegistrosContabeis, opt => opt.MapFrom(src => src.RegistrosContabeis))
            .ForMember(dest => dest.TotalRegistros, opt => opt.MapFrom(src => src.RegistrosContabeis.Count))
            .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.RegistrosContabeis.Sum(r => r.Valor)));

        CreateMap<CriarCentroCustoDto, CentroCusto>()
            .ForMember(dest => dest.IdCentroCusto, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrosContabeis, opt => opt.Ignore());

        CreateMap<AtualizarCentroCustoDto, CentroCusto>()
            .ForMember(dest => dest.IdCentroCusto, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrosContabeis, opt => opt.Ignore());

        // Mapeamentos para Conta
        CreateMap<Conta, ContaDto>()
            .ForMember(dest => dest.TipoDescricao, opt => opt.MapFrom(src => src.GetTipoDescricao()))
            .ForMember(dest => dest.QuantidadeRegistros, opt => opt.MapFrom(src => src.RegistrosContabeis.Count))
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore());

        CreateMap<Conta, ContaDetalhesDto>()
            .ForMember(dest => dest.TipoDescricao, opt => opt.MapFrom(src => src.GetTipoDescricao()))
            .ForMember(dest => dest.RegistrosContabeis, opt => opt.MapFrom(src => src.RegistrosContabeis))
            .ForMember(dest => dest.TotalRegistros, opt => opt.MapFrom(src => src.RegistrosContabeis.Count))
            .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.RegistrosContabeis.Sum(r => r.Valor)));

        CreateMap<CriarContaDto, Conta>()
            .ForMember(dest => dest.IdContaContabil, opt => opt.Ignore())
            .ForMember(dest => dest.ClienteIdCliente, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrosContabeis, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.Ignore());

        CreateMap<AtualizarContaDto, Conta>()
            .ForMember(dest => dest.IdContaContabil, opt => opt.Ignore())
            .ForMember(dest => dest.ClienteIdCliente, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrosContabeis, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.Ignore());

        // Mapeamentos para RegistroContabil
        CreateMap<RegistroContabil, RegistroContabilDto>()
            .ForMember(dest => dest.NomeConta, opt => opt.MapFrom(src => src.Conta.NomeContaContabil))
            .ForMember(dest => dest.NomeCentroCusto, opt => opt.MapFrom(src => src.CentroCusto.NomeCentroCusto));

        CreateMap<RegistroContabil, RegistroContabilDetalhesDto>()
            .ForMember(dest => dest.Conta, opt => opt.MapFrom(src => src.Conta))
            .ForMember(dest => dest.CentroCusto, opt => opt.MapFrom(src => src.CentroCusto));

        CreateMap<CriarRegistroContabilDto, RegistroContabil>()
            .ForMember(dest => dest.IdRegCont, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Conta, opt => opt.Ignore())
            .ForMember(dest => dest.CentroCusto, opt => opt.Ignore())
            .ForMember(dest => dest.Vendas, opt => opt.Ignore());

        CreateMap<AtualizarRegistroContabilDto, RegistroContabil>()
            .ForMember(dest => dest.IdRegCont, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Conta, opt => opt.Ignore())
            .ForMember(dest => dest.CentroCusto, opt => opt.Ignore())
            .ForMember(dest => dest.Vendas, opt => opt.Ignore());

        // Mapeamentos para Cliente
        CreateMap<Cliente, ClienteDto>()
            .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo.ToString()));

        CreateMap<CriarClienteDto, Cliente>()
            .ForMember(dest => dest.IdCliente, opt => opt.Ignore())
            .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
            .ForMember(dest => dest.Contas, opt => opt.Ignore())
            .ForMember(dest => dest.Vendas, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Ativo) ? src.Ativo[0] : 'S'));

        CreateMap<AtualizarClienteDto, Cliente>()
            .ForMember(dest => dest.IdCliente, opt => opt.Ignore())
            .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
            .ForMember(dest => dest.CpfCnpj, opt => opt.Ignore())
            .ForMember(dest => dest.Senha, opt => opt.Ignore())
            .ForMember(dest => dest.Contas, opt => opt.Ignore())
            .ForMember(dest => dest.Vendas, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Ativo) ? src.Ativo[0] : 'S'));

        // Mapeamentos para Vendas
        CreateMap<Vendas, VendasDto>()
            .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Cliente.NomeCliente))
            .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.RegistroContabil.Valor));

        CreateMap<CriarVendasDto, Vendas>()
            .ForMember(dest => dest.IdVendas, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.Ignore())
            .ForMember(dest => dest.RegistroContabil, opt => opt.Ignore());

        CreateMap<AtualizarVendasDto, Vendas>()
            .ForMember(dest => dest.IdVendas, opt => opt.Ignore())
            .ForMember(dest => dest.Cliente, opt => opt.Ignore())
            .ForMember(dest => dest.RegistroContabil, opt => opt.Ignore());
    }
}
