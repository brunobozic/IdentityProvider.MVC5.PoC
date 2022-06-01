using Confluent.Kafka;

namespace KafkaModule.Core.DTOs
{
    public class ConsumeMessageResult
    {
        public string Message { get; set; }
        public long Offset { get; set; }
        public int Partition { get; set; }
        public string Topic { get; set; }

        public long LowOffset { get; set; }
        public long HiOffset { get; set; }

        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;

        //public TopicPartitionOffset CurrentTopicPartitionOffset { get; set; }
        public long CurrentOffset { get; set; }

        public string ErrorType { get; set; }
        public ConsumeResult<string, string> CompleteMessage { get; set; }
        public int CurrentPartition { get; set; }
        public string HeaderX { get; set; }
    }
}