using System;
using System.Runtime.Serialization;

namespace Logging.WCF.Repository.EF.DomainBaseAbstractions
{
    [Serializable]
    internal class EntityIsInvalidException : Exception
    {
        public EntityIsInvalidException()
        {
        }

        public EntityIsInvalidException(string message) : base(message)
        {
        }

        public EntityIsInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityIsInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}