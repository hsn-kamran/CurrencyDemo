using AutoMapper;
using CurrencyDemo.Models;

namespace CurrencyDemo;

public class MapperConfig
{
    public static Mapper InitializeAutomapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Valute, Currency>()
                .ForMember(d => d.CurrencyId, opt => opt.MapFrom(s => s.Id));
        });
        
        var mapper = new Mapper(config);

        return mapper;
    }
}
