using BadBroker.Logic.DTO;
using BadBroker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BadBroker.Extension
{
    public static class Extensions
    {
        public static BestStrategyViewModel DistinctBestStrategy(this IEnumerable<BestStrategyViewModel> sourceCollections)
        {
            var reciverCollection = new BestStrategyViewModel();
            var revenueMax = decimal.MinValue;

            foreach(var source in sourceCollections)
            {
                foreach(var rate in source.Rates)
                {
                    if (reciverCollection.Rates.Any(x => x.Date == rate.Date))
                    {
                        if(source.Tool.Equals(CurrencyEnum.RUB.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            reciverCollection.Rates.Where(x => x.Date == rate.Date).ToList().ForEach(x => x.Rub = rate.Rub);
                        }
                        else if (source.Tool.Equals(CurrencyEnum.EUR.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            reciverCollection.Rates.Where(x => x.Date == rate.Date).ToList().ForEach(x => x.Eur = rate.Eur);
                        }
                        else if (source.Tool.Equals(CurrencyEnum.GBP.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            reciverCollection.Rates.Where(x => x.Date == rate.Date).ToList().ForEach(x => x.Gbp = rate.Gbp);
                        }
                        else if (source.Tool.Equals(CurrencyEnum.JPY.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            reciverCollection.Rates.Where(x => x.Date == rate.Date).ToList().ForEach(x => x.Jpy = rate.Jpy);
                        }
                    }
                    else
                        reciverCollection.Rates.Add(new RateViewModel
                        {
                            Date = rate.Date,
                            Rub = rate.Rub,
                            Eur = rate.Eur,
                            Gbp = rate.Gbp,
                            Jpy = rate.Jpy,
                        });

                    if (source.Revenue > revenueMax)
                    {
                        revenueMax = source.Revenue;

                        reciverCollection.Tool = source.Tool;
                        reciverCollection.Revenue = source.Revenue;
                        reciverCollection.BuyDate = source.BuyDate;
                        reciverCollection.SellDate = source.SellDate;
                    }
                }
            }

            return reciverCollection;
        }
    }
}
