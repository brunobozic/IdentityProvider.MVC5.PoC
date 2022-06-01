using IdentityProvider.Web.MVC6.AppConfiguration;

namespace IdentityProvider.Web.MVC6
{
    public class PollySettings
    {
        public KafkaConsumerCircuitBreakerPolicy KafkaConsumerCircuitBreakerPolicy { get; set; }

        public KafkaConsumerRetryPolicy KafkaConsumerRetryPolicy { get; set; }

        public KafkaProducerCircuitBreakerPolicy KafkaProducerCircuitBreakerPolicy { get; set; }

        public KafkaProducerRetryPolicy KafkaProducerRetryPolicy { get; set; }

        public KafkaSharedExponentialBackoff KafkaSharedExponentialBackoff { get; set; }
    }
}