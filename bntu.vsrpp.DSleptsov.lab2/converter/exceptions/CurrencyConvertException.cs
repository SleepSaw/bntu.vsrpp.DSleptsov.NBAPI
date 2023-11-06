using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace bntu.vsrpp.DSleptsov.lab2.converter.exceptions
{
    public class CurrencyConvertException : Exception
    {
        public CurrencyConvertException()
        {
        }

        public CurrencyConvertException(string? message) : base(message)
        {
        }

        public CurrencyConvertException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CurrencyConvertException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
