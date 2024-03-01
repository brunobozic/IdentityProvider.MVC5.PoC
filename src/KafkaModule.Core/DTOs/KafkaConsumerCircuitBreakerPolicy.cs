namespace KafkaModule.Core.DTOs
{
    public class KafkaConsumerCircuitBreakerPolicy
    {
        public int Tries { get; set; }
        public int CooldownSeconds { get; set; }
    }
}