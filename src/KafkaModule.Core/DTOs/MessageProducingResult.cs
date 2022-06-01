using Confluent.Kafka;

namespace KafkaModule.Core.DTOs
{
    internal class MessageProducingResult
    {
        public string Message { get; set; }
        public string MessageId { get; set; }
        public long Offset { get; set; }
        public int Partition { get; set; }
        public string Topic { get; set; }

        public long LowOffset { get; set; }
        public long HiOffset { get; set; }

        public bool Success { get; set; } = false;
        public string ProducedStatusMessage { get; set; } = string.Empty;
        public TopicPartitionOffset CurrentTopicPartitionOffset { get; set; }
        public long CurrentOffset { get; set; }
        public string ErrorType { get; set; }
    }
}