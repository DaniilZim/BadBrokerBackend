using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BadBroker.DAL.Model
{
    public class Rate
    {
        public int Id { get; set; }

        public DateTime DateTrunc { get; set; }

        public decimal Value { get; set; }

        public int CurrencyPairId { get; set; }

        public CurrencyPair CurrencyPair { get; set; }
    }
}
