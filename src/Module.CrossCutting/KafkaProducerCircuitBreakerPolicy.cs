namespace Module.CrossCutting
{
    public class KafkaProducerCircuitBreakerPolicy
    {
        public int Tries { get; set; }
        public int CooldownSeconds { get; set; }
    }
}