using AutoMapper;
using BadBroker.Logic.DTO;
using BadBroker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BadBroker.MapperProfile
{
    public class ViewProfile : Profile
    {
        public ViewProfile()
        {
            //MapRate();
            MapBestStrategy();
        }

        public void MapBestStrategy()
        {
            CreateMap<BestStrategyViewModel, BestStrategyDto>()
                .ForMember(dest => dest.Tool, opt => opt.MapFrom(src => Enum.Parse(typeof(CurrencyEnum), src.Tool)))
                .ForMember(dest => dest.Rates, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Tool, opt => opt.MapFrom(src => src.Tool.ToString()))
                .ForMember(dest => dest.Rates, opt => opt.Ignore())
                .ForMember(dest => dest.BuyDate, opt => opt.MapFrom(src => src.BuyDate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.SellDate, opt => opt.MapFrom(src => src.SellDate.ToString("yyyy-MM-dd") ))
                .AfterMap((src, dest) => 
                {
                    var rates = new List<RateViewModel>();
                    
                    foreach (var rate in src.Rates)
                    {
                        var rateVm = new RateViewModel { Date = rate.DateTrunc.ToString("yyyy-MM-dd")};

                        if (src.Tool == CurrencyEnum.RUB)
                            rateVm.Rub = rate.Value;
                        else if (src.Tool == CurrencyEnum.EUR)
                            rateVm.Eur = rate.Value;
                        else if (src.Tool == CurrencyEnum.JPY)
                            rateVm.Jpy = rate.Value;
                        else if (src.Tool == CurrencyEnum.GBP)
                            rateVm.Gbp = rate.Value;

                        rates.Add(rateVm);
                    }

                    dest.Rates = rates;
                });
        }

        /*
        public void MapRate()
        {
            CreateMap<RateViewModel, RateDto>()
                .ForMember(dest => dest.DateTrunc, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Value, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.DateTrunc))
                .ForMember(dest => dest.Rub, opt => opt.Ignore())
                .ForMember(dest => dest.Eur, opt => opt.Ignore())
                .ForMember(dest => dest.Jpy, opt => opt.Ignore())
                .ForMember(dest => dest.Gbp, opt => opt.Ignore())
                ;
        }
        */
    }
}
