using System.ComponentModel;

namespace KafkaModule.Core.Enums
{
    public enum SaslMechanismEnum
    {
        [Description("PLAIN")]
        Plain,

        [Description("GSSAPI")]
        GSSAPI,

        [Description("SCRAMSHA256")]
        SCRAMSHA256
    }
}