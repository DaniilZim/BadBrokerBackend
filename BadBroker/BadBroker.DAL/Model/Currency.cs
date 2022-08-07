using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BadBroker.DAL.Model
{
    public class Currency
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public List<CurrencyPair> CurrencyBasePairs { get; set; } = new List<CurrencyPair>();

        public List<CurrencyPair> CurrencyCounterPairs { get; set; } = new List<CurrencyPair>();
    }
}
