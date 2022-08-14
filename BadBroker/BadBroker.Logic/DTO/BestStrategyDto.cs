using System;
using System.Collections.Generic;
using System.Text;

namespace BadBroker.Logic.DTO
{
    public class BestStrategyDto
    {
        public IEnumerable<CurrencyPairRateDto> CurrencyPairRates { get; set; } = new List<CurrencyPairRateDto>();
    }
}
