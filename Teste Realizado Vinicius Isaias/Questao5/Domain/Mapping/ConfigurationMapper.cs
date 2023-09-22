using AutoMapper;
using Questao5.Domain.Entities;
using Questao5.Domain.ViewModel;
using System.Globalization;

namespace Questao5.Domain.Mapping
{
    public class ConfigurationMapper : Profile
    {
        public ConfigurationMapper()
        {
            CreateMap<ContaCorrente, MovimentacaoContaDto>().ReverseMap();
            CreateMap<ContaCorrente, SaldoDto>().ForMember(destino => destino.Saldo, origem => origem.MapFrom(dados => dados.Saldo.ToString("#0.00", CultureInfo.InvariantCulture)));
        }
    }
}
