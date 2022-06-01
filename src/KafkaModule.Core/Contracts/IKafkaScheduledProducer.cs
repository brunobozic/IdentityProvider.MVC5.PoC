using Confluent.Kafka;

namespace KafkaModule.Core.Contracts
{
    public interface IKafkaScheduledProducer : IDisposable
    {
        Task WriteMessageAsync(string message);

        Handle UnderlyingHandle();

        IProducer<string, string> UnderlyingProducerInstance();
    }
}