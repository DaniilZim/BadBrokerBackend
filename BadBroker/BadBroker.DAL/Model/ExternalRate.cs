using System;
using System.Collections.Generic;
using System.Text;

namespace BadBroker.DAL.Model
{
    public class ExternalRate
    {
        public string Disclaimer { get; set; }
        public string License { get; set; }
        public long TimeStamp { get; set; }
        public string Base { get; set; }
        public CounterRates Rates { get; set; }

    }

    public class CounterRates
    {
        public decimal RUB { get; set; }
        public decimal JPY { get; set; }
        public decimal EUR { get; set; }
        public decimal GBP { get; set; }
    }

}
