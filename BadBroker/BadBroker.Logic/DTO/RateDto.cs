using System;
using System.Collections.Generic;
using System.Text;

namespace BadBroker.Logic.DTO
{
    public class RateDto
    {
        public int Id { get; set; }

        public DateTime DateTrunc { get; set; }

        public decimal Value { get; set; }
    }
}
