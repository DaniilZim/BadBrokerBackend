using System;
using System.Collections.Generic;

namespace BadBroker.ViewModel
{
    public class BestStrategyViewModel
    {
        public string BuyDate { get; set; }

        public string SellDate { get; set; }

        public string Tool { get; set; }

        public decimal Revenue { get; set; }

        public List<RateViewModel> Rates { get; set; } = new List<RateViewModel>();
    }
}
