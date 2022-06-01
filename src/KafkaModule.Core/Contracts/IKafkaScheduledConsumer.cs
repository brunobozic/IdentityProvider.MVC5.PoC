using Confluent.Kafka;
using KafkaModule.Core.DTOs;

namespace KafkaModule.Core.Contracts
{
    /// <summary>
    ///     Begins consuming a set Kafka topic and reporting read messages to persistor producer
    /// </summary>
    public interface IKafkaScheduledConsumer : IDisposable
    {
        /// <summary>
        /// Reads a single message from Kafka topicpartition assignments
        /// Always returns a result.
        /// Does not throw.
        /// Evaluate the returned result to decide what to do next.
        /// </summary>
        /// <returns></returns>
        ConsumeMessageResult Consume();

        void Pause();

        void Continue();

        string GetTopic();

        List<string> UnderlyingSubscriptions();

        Handle UnderlyingHandle();

        /// <summary>
        /// Underlying consumer instance
        /// </summary>
        /// <returns></returns>
        IConsumer<string, string> Instance();

        /// <summary>
        /// Store the last message that was successfuly read and persisted to the Donat database.
        /// </summary>
        void StoreOffsetFor(ConsumeResult<string, string> msg);

        bool SkipPoisonPill(ConsumeResult<string, string> consumedMessage);

        /// <summary>
        /// Skips a message if we fail to read it by seeking a position +1
        /// </summary>
        /// <param name="topicPartition"></param>
        /// <param name="recordOffset"></param>
        /// <returns></returns>
        bool SkipPoisonPill(TopicPartition topicPartition, long recordOffset);

        /// <summary>
        /// Seek to a specified position (offset)
        /// </summary>
        /// <param name="topicPartition"></param>
        /// <param name="recordOffset"></param>
        void Seek(TopicPartition topicPartition, long recordOffset);

        string GetCurrentOffset();

        string GetBootstrapServers();

        string GetKafkaConsumerMaxOffset();
    }
}