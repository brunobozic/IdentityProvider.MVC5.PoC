namespace Module.CrossCutting
{
    public interface IKafkaLoggingProducerSettings
    {
        string BootstrapServers { get; set; }
        string SaslUsername { get; set; }
        string SaslPassword { get; set; }
        string ClientId { get; set; }
        string SecurityProtocol { get; set; }
        string SaslMechanism { get; set; }
        bool? Debug { get; set; }
    }
}