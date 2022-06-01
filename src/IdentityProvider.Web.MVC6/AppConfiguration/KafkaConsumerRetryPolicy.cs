namespace IdentityProvider.Web.MVC6.AppConfiguration
{
    public class KafkaConsumerRetryPolicy
    {
        public int RetryTimes { get; set; }
        public KafkaSharedExponentialBackoff ExponentialBackoff { get; set; }
    }
}