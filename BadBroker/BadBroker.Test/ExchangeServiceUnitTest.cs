using System;
using Xunit;
using Moq;
using BadBroker.Logic.Service;
using Microsoft.Extensions.Logging;
using BadBroker.DAL.Repository;
using AutoMapper;
using BadBroker.Logic.DTO;
using BadBroker.DAL.Model;
using System.Collections.Generic;
using System.Linq;

namespace BadBroker.Test
{
    public class ExchangeServiceUnitTest
    {
        readonly IExchangeService service;

        Mock<IRateRepository> _rateMock = new Mock<IRateRepository>();
        Mock<IRateExternalRepository> _rateExternalMock = new Mock<IRateExternalRepository>();

        public ExchangeServiceUnitTest()
        {
            var _loggerMock = new Mock<ILogger<IExchangeService>>();
            var _mapperMock = new Mock<IMapper>();

            _mapperMock.Setup(x => x.Map<IEnumerable<Rate>, IEnumerable<RateDto>>(It.IsAny<IEnumerable<Rate>>()))
                .Returns<IEnumerable<Rate>>(x => x.Select(r => new RateDto { DateTrunc = r.DateTrunc, Value = r.Value }));

            service = new ExchangeService(_loggerMock.Object, _rateMock.Object, _rateExternalMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void GetBestStrategy_Success()
        {
            //Arrange
            DateTime startDate = DateTime.Now.Date.AddDays(-3);
            DateTime endDate = DateTime.Now.Date;
            decimal money = 1000;
            string baseCurrency = "USD";

            int currencyPairId = 100;

            var rates = new List<Rate>
                {
                    new Rate { CurrencyPairId = currencyPairId, DateTrunc = DateTime.Now.Date.AddDays(-10), Value = 10 },
                    new Rate { CurrencyPairId = currencyPairId, DateTrunc = DateTime.Now.Date.AddDays(-8), Value = 60 },
                    new Rate { CurrencyPairId = currencyPairId, DateTrunc = DateTime.Now.Date.AddDays(-6), Value = 60 },
                    new Rate { CurrencyPairId = currencyPairId, DateTrunc = DateTime.Now.Date.AddDays(-4), Value = 60 },
                    new Rate { CurrencyPairId = currencyPairId, DateTrunc = DateTime.Now.Date.AddDays(-2), Value = 60 },
                    new Rate { CurrencyPairId = currencyPairId, DateTrunc = DateTime.Now.Date.AddDays( 0), Value = 60 },
                };

            _rateMock.Setup(x => x.GetCurrencyByKey(It.IsAny<string>()))
                .Returns<string>(x => new Currency { Id = 1, Key = x, Name = x });

            _rateMock.Setup(x => x.GetCurrencies())
                .Returns(new Currency[] { 
                    new Currency { Id = 1, Name = "USD", Key = "USD" },
                    new Currency { Id = 2, Name = "RUB", Key = "RUB" },
                });

            _rateMock.Setup(x => x.GetCurrencyPair(It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int>((x, y) => new CurrencyPair {
                    Id = 100,
                    BaseCurrencyId = x,
                    CounterCurrencyId = y,
                });

            _rateMock.Setup(x => x.GetRates(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns<int, DateTime, DateTime>((i, s, e) => rates
                    .Where(c => c.DateTrunc >= s && c.DateTrunc <= e).ToList());

            _rateExternalMock.Setup(x => x.GetRates(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns<string, DateTime>((s, d) => new ExternalRate
                {
                    Base = s,
                    TimeStamp = d.Ticks,
                    Rates = new CounterRates { RUB = 55 },
                });

            _rateMock.Setup(x => x.AddRate(It.IsAny<Rate>())).Callback((Rate r) => rates.Add(r));

            //Act
            var result = service.GetBestStrategy(startDate, endDate, money, baseCurrency);

            //Assert
            Assert.NotNull(result);
        }
    }
}
