namespace Module.CrossCutting
{
    public class KafkaConsumerCircuitBreakerPolicy
    {
        public int Tries { get; set; }
        public int CooldownSeconds { get; set; }
    }
}