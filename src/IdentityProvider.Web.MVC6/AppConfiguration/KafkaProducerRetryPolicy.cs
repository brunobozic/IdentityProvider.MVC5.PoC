namespace IdentityProvider.Web.MVC6.AppConfiguration
{
    public class KafkaProducerRetryPolicy
    {
        public int RetryTimes { get; set; }
        public KafkaSharedExponentialBackoff ExponentialBackoff { get; set; }
    }
}