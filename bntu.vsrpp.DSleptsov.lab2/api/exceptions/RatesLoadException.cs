using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bntu.vsrpp.DSleptsov.lab2.api.exceptions
{
    public class RatesLoadException : Exception
    {
        public RatesLoadException()
        {
        }

        public RatesLoadException(string? message) : base(message)
        {
        }

        public RatesLoadException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RatesLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
