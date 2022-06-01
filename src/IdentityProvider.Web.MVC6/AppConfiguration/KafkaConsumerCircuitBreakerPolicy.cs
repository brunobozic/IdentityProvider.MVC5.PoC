namespace IdentityProvider.Web.MVC6.AppConfiguration
{
    public class KafkaConsumerCircuitBreakerPolicy
    {
        public int Tries { get; set; }
        public int CooldownSeconds { get; set; }
    }
}