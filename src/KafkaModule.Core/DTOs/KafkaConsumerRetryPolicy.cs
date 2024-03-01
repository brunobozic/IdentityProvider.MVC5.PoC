namespace KafkaModule.Core.DTOs
{
    public class KafkaConsumerRetryPolicy
    {
        public int RetryTimes { get; set; }
        public KafkaSharedExponentialBackoff ExponentialBackoff { get; set; }
    }
}