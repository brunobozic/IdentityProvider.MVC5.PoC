namespace KafkaModule.Core.DTOs
{
    public class KafkaProducerCircuitBreakerPolicy
    {
        public int Tries { get; set; }
        public int CooldownSeconds { get; set; }
    }
}