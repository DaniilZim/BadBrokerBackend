using AutoMapper;
using BadBroker.DAL.Model;
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
            MapBestStrategy();
        }

        public void MapBestStrategy()
        {
            CreateMap<BestStrategyViewModel, BestStrategyDto>()
                .ForMember(dest => dest.CurrencyPairRates, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Rates, opt => opt.Ignore())
                .AfterMap((src, dest) => 
                {
                    dest.Revenue = decimal.MinValue;

                    var rates = new List<RateViewModel>();

                    foreach (var cpr in src.CurrencyPairRates.OrderBy(x => x.CounterCurrencyId))
                    {
                        foreach (var rate in cpr.Rates)
                        {
                            var rateVM = rates.SingleOrDefault(x => x.Date == rate.DateTrunc.ToString("yyyy-MM-dd"));

                            if (rateVM == default)
                            {
                                rateVM = new RateViewModel();
                                rateVM.Date = rate.DateTrunc.ToString("yyyy-MM-dd");

                                rates.Add(rateVM);
                            }

                            if (cpr.Tool == CurrencyEnum.RUB)
                            {
                                rateVM.Rub = rate.Value;
                            }
                            else if (cpr.Tool == CurrencyEnum.EUR)
                            {
                                rateVM.Eur = rate.Value;
                            }
                            else if (cpr.Tool == CurrencyEnum.GBP)
                            {
                                rateVM.Gbp = rate.Value;
                            }
                            else if (cpr.Tool == CurrencyEnum.JPY)
                            {
                                rateVM.Jpy = rate.Value;
                            }
                        }

                        if (dest.Revenue < cpr.Revenue)
                        {
                            dest.Revenue = cpr.Revenue;
                            dest.Tool = cpr.Tool.ToString();
                            dest.BuyDate = cpr.BuyDate.ToString("yyyy-MM-dd");
                            dest.SellDate = cpr.SellDate.ToString("yyyy-MM-dd");
                        }
                    }

                    dest.Rates = rates;
                });
        }
    }
}
