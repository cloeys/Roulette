using System;
using System.Runtime.Serialization;

namespace Roulette.Exceptions
{
    [Serializable]
    public class RouletteException : Exception
    {
        public RouletteException()
        {
        }

        public RouletteException(string message) : base(message)
        {
        }

        public RouletteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RouletteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}