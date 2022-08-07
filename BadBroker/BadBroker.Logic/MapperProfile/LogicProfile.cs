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
        }

        public void MapRate()
        {
            CreateMap<RateDto, Rate>()
                .ForMember(s => s.CurrencyPairId, d => d.Ignore())
                .ForMember(s => s.CurrencyPair, d => d.Ignore())
                .ReverseMap()
                ;
        }
    }
}
