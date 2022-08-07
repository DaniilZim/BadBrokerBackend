using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadBroker.DAL.Model
{
    public class CurrencyPair
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? BaseCurrencyId { get; set; }

        public Currency BaseCurrency { get; set; }

        public int? CounterCurrencyId { get; set; }

        public Currency CounterCurrency { get; set; }

        public ICollection<Rate> Rates { get; set; } = new List<Rate>();
    }
}
