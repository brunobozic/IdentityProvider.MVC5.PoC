namespace IdentityProvider.Web.MVC6
{
    public class KafkaConsumerSettings
    {
        public string BootstrapServers { get; set; }
        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
        public string SaslMechanism { get; set; }
        public string SecurityProtocol { get; set; }
        public string SecurityMechanism { get; set; }
        public string GroupId { get; set; }
        public string ClientId { get; set; }
        public string AutoOffsetReset { get; set; }
        public string SaslKerberosServiceName { get; set; }
        public string SaslKerberosKinitCmd { get; set; }
        public string SslCaLocation { get; set; }
        public string SslCertificateLocation { get; set; }
        public string SslKeyLocation { get; set; }
        public string SaslKerberosKeytab { get; set; }
        public string SaslKerberosPrincipal { get; set; }
        public bool? EnableAutoOffsetStore { get; set; }
        public bool? EnablePartitionEof { get; set; }
        public string Debug { get; set; }
        public int? MetadataRequestTimeoutMs { get; set; }
        public string KafkaTopic { get; set; }
        public bool? EnableAutoCommit { get; set; }
        public bool ApiVersionRequest { get; set; }
        public string KerberosPrincipal { get; set; }
        public bool UseHardcodedOffset { get; set; }
        public int HardcodedOffset { get; set; }
    }
}