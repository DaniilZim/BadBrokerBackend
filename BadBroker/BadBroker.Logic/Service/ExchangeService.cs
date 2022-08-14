using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BadBroker.DAL.Repository;
using BadBroker.Logic.DTO;
using Microsoft.Extensions.Logging;
using AutoMapper;
using BadBroker.DAL.Model;
using BadBroker.Logic.Exceptions;

namespace BadBroker.Logic.Service
{
    public interface IExchangeService
    {
        BestStrategyDto GetBestStrategy(DateTime startDate, DateTime endDate, decimal money, string baseCurrency);
    }

    public class ExchangeService : IExchangeService
    {
        const int MIN_PERIOD = 1;
        const int MAX_PERIOD = 60;
        const int BROKER_PRICE = 1;

        private readonly ILogger<IExchangeService> _logger;
        private readonly IRateRepository _rateRepository;
        private readonly IRateExternalRepository _rateExternalRepository;
        private readonly IMapper _mapper;

        public ExchangeService(ILogger<IExchangeService> logger,
            IRateRepository rateRepository,
            IRateExternalRepository rateExternalRepository,
            IMapper mapper)
        {
            _logger = logger;
            _rateRepository = rateRepository;
            _rateExternalRepository = rateExternalRepository;
            _mapper = mapper;
        }

        public BestStrategyDto GetBestStrategy(DateTime startDate, DateTime endDate, decimal money, string baseCurrencyKey)
        {
            try
            {
                if ((endDate - startDate).Days < MIN_PERIOD)
                    throw new TooSmallDateIntervalException($"Period must be more than or equal to {MIN_PERIOD} days");//TODO: to resource

                if ((endDate - startDate).Days > MAX_PERIOD)
                    throw new TooBigDateIntervalException($"Period must be less than or equal to {MAX_PERIOD} days");

                var baseCurrency = _rateRepository.GetCurrencyByKey(baseCurrencyKey);

                if (baseCurrency == null)
                    throw new BaseCurrencyNotFoundException();

                var counterCurrencies = _rateRepository.GetCurrencies()
                    .Where(x => x.Key != baseCurrencyKey);

                if (counterCurrencies == null || !counterCurrencies.Any())
                    throw new CounterCurrencyNotFoundException();

                var results = new List<CurrencyPairRateDto>();

                foreach (var counterCurrency in counterCurrencies)
                {
                    var currencyPair = _rateRepository.GetCurrencyPair(baseCurrency.Id, counterCurrency.Id);

                    if (currencyPair == null)
                        throw new CounterCurrencyPairNotFoundException($"{baseCurrency.Key}/{counterCurrency.Key}");

                    var rates = _rateRepository.GetRates(currencyPair.Id, startDate, endDate);

                    for (var currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
                    {
                        if (!rates.Any(x => x.DateTrunc == currentDate))
                        {
                            var externalRates = _rateExternalRepository.GetRates(baseCurrencyKey, currentDate);

                            var newRate = new Rate
                            {
                                CurrencyPairId = currencyPair.Id,
                                DateTrunc = currentDate,
                                Value =
                                counterCurrency.Key == nameof(externalRates.Rates.RUB) ? externalRates.Rates.RUB :
                                counterCurrency.Key == nameof(externalRates.Rates.JPY) ? externalRates.Rates.JPY :
                                counterCurrency.Key == nameof(externalRates.Rates.GBP) ? externalRates.Rates.GBP :
                                counterCurrency.Key == nameof(externalRates.Rates.EUR) ? externalRates.Rates.EUR : 0M
                            };

                            //Cashing to DB
                            _rateRepository.AddRate(newRate);

                            rates.Add(newRate);
                        }
                    }

                    if (!rates.Any())
                        continue;

                    var result = CalculateBestStrategy(_mapper.Map<IEnumerable<Rate>, IEnumerable <RateDto>>(rates), money);

                    if (Enum.TryParse<CurrencyEnum>(counterCurrency.Key, ignoreCase: true, out var _enum))
                        result.Tool = _enum;
                    else
                        throw new ArgumentOutOfRangeException(counterCurrency.Key);

                    results.Add(result);
                }

                return new BestStrategyDto { CurrencyPairRates = results };
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        private CurrencyPairRateDto CalculateBestStrategy(IEnumerable<RateDto> rates, decimal money)
        {
            if (rates == null)
                throw new ArgumentNullException();

            var result = new CurrencyPairRateDto { Rates = rates };
            var dateBuy = result.BuyDate = rates.Min(x => x.DateTrunc);
            var dateSell = result.SellDate = rates.Max(x => x.DateTrunc);
            var revenue = result.Revenue = decimal.MinValue;
            var cost = BROKER_PRICE;// TODO: to config

            foreach (var rateIn in rates.OrderBy(x => x.DateTrunc))
                foreach (var rateOut in rates
                    .Where(x => x.DateTrunc > rateIn.DateTrunc)
                    .OrderBy(x => x.DateTrunc)
                )
                {
                    var dayLength = (rateOut.DateTrunc - rateIn.DateTrunc).Days;

                    var priceBroker = dayLength * cost;

                    revenue = money - (money * rateIn.Value / rateOut.Value - priceBroker);
                    
                    if(revenue > result.Revenue)
                    {
                        result.Revenue = revenue;
                        result.BuyDate = rateIn.DateTrunc;
                        result.SellDate = rateOut.DateTrunc;
                    }
                }
            
            return result;
        }
    }
}
