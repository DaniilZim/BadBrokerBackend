using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BadBroker.DAL.Model;
using BadBroker.Logic.DTO;

namespace BadBroker.Logic.MapperProfile
{
    public class LogicProfile : Profile
    {
        public LogicProfile()
        {
            MapRate();
            MapCurrencyPair();
        }

        public void MapRate()
        {
            CreateMap<RateDto, Rate>()
                .ForMember(s => s.CurrencyPairId, d => d.Ignore())
                .ForMember(s => s.CurrencyPair, d => d.Ignore())
                .ReverseMap()
                ;
        }

        public void MapCurrencyPair()
        {
            CreateMap<CurrencyPairRateDto, CurrencyPair>()
                .ForMember(s => s.Id, d => d.Ignore())
                .ForMember(s => s.BaseCurrency, d => d.Ignore())
                .ForMember(s => s.CounterCurrency, d => d.Ignore())
                .ForMember(s => s.BaseCurrencyId, opt => opt.MapFrom(d => (int)d.BaseCurrencyId))
                .ForMember(s => s.CounterCurrencyId, opt => opt.MapFrom(d => (int)d.CounterCurrencyId))
                .ReverseMap()
                .ForMember(s => s.BaseCurrencyId, opt => opt.MapFrom(d => (CurrencyEnum)d.BaseCurrencyId))
                .ForMember(s => s.CounterCurrencyId, opt => opt.MapFrom(d => (CurrencyEnum)d.CounterCurrencyId))
                ;
        }
    }
}
