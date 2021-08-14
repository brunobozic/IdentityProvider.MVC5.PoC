using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logging.WCF.Infrastructure.BusinessRules
{
    public class ModelValidationException : Exception
    {
        public ModelValidationException()
        {
        }


        public ModelValidationException(string message)
            : base(message)
        {
        }


        public ModelValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        public ModelValidationException(string message, Exception innerException,
            IEnumerable<ValidationResult> validationErrors)
            : base(message, innerException)
        {
            ValidationErrors = validationErrors;
        }


        public ModelValidationException(string message, IEnumerable<ValidationResult> validationErrors)
            : base(message)
        {
            ValidationErrors = validationErrors;
        }


        protected ModelValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public IEnumerable<ValidationResult> ValidationErrors { get; }
    }
}