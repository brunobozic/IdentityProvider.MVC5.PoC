using System.ComponentModel;

namespace KafkaModule.Core.Enums
{
    public enum SecurityProtocolEnum
    {
        [Description("PLAINTEXT")]
        PlainText,

        [Description("SASL_PLAINTEXT")]
        SASL_Plaintext,

        [Description("SASLSSL")]
        Kerberos,

        [Description("SASLSSLSCRAM")]
        SASL_ssl,
    }
}