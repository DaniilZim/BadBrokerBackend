using System;
using System.Collections.Generic;
using System.Text;

namespace BadBroker.Logic.DTO
{
    public class BestStrategyDto
    {
        public BestStrategyDto()
        {
            Rates = new List<RateDto>();
        }

        public DateTime BuyDate { get; set; }

        public DateTime SellDate { get; set; }

        public CurrencyEnum Tool { get; set; }

        public decimal Revenue { get; set; }

        public IEnumerable<RateDto> Rates { get; set; }
    }
}
