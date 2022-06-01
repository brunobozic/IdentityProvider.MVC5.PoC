using System;

namespace Module.CrossCutting.Domain
{
    public class ValueObjectIsInvalidException : Exception
    {
        public ValueObjectIsInvalidException(string message)
            : base(message)
        {
        }
    }
}