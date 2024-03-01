namespace Module.CrossCutting
{
    public interface IMyConfigurationValues
    {
        string Environment { get; set; }
        KafkaConsumerSettings KafkaConsumerSettings { get; set; }
        KafkaProducerSettings KafkaProducerSettings { get; set; }
        KafkaLoggingProducerSettings KafkaLoggingProducerSettings { get; set; }
        PollySettings PollySettings { get; set; }

        DeadLetterArchiveJobSettings DeadLetterArchiveJobSettings { get; set; }

        DeadLetterOutboxJobSettings DeadLetterOutboxJobSettings { get; set; }
    }
}