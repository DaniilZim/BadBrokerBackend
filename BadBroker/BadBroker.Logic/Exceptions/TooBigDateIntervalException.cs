using System;

namespace BadBroker.Logic.Exceptions
{
    public class TooBigDateIntervalException : ApplicationException
    {
        public TooBigDateIntervalException(string message) : base(message)
        { }
    }
}
