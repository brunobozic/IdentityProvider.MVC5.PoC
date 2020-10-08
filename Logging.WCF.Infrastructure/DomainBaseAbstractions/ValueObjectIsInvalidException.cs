using System;
using System.Runtime.Serialization;

namespace Logging.WCF.Repository.EF.DomainBaseAbstractions
{
    [Serializable]
    internal class ValueObjectIsInvalidException : Exception
    {
        public ValueObjectIsInvalidException()
        {
        }

        public ValueObjectIsInvalidException(string message) : base(message)
        {
        }

        public ValueObjectIsInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ValueObjectIsInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}