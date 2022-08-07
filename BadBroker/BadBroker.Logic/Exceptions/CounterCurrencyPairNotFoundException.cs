using System;

namespace BadBroker.Logic.Exceptions
{
    public class CounterCurrencyPairNotFoundException : ApplicationException
    {
        public CounterCurrencyPairNotFoundException(string message) : base(message)
        { }
    }
}
