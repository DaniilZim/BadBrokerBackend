using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadBroker.Logic.DTO
{
    public class CurrencyPairRateDto
    {
        public DateTime BuyDate { get; set; }

        public DateTime SellDate { get; set; }

        public CurrencyEnum Tool { get; set; }

        public decimal Revenue { get; set; }

        public string Name { get; set; }

        public CurrencyEnum BaseCurrencyId { get; set; }

        public CurrencyEnum CounterCurrencyId { get; set; }

        public IEnumerable<RateDto> Rates { get; set; } = new List<RateDto>();
    }
}
