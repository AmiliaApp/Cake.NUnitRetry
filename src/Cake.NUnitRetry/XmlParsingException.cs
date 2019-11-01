using System;
using System.Runtime.Serialization;

namespace Cake.NUnitRetry
{
    public class XmlParsingException : Exception
    {
        public XmlParsingException()
        {
        }

        public XmlParsingException(string message) : base(message)
        {
        }

        public XmlParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XmlParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
