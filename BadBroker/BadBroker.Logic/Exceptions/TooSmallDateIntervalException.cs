using System;

namespace BadBroker.Logic.Exceptions
{
    public class TooSmallDateIntervalException : ApplicationException
    {
        public TooSmallDateIntervalException(string message) : base(message)
        { }
    }
}
