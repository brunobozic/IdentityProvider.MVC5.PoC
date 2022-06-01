using Confluent.Kafka;

namespace KafkaModule.Core.Contracts
{
    public interface IKafkaLoggingProducer : IDisposable
    {
        Task WriteLogMessageAsync(string message);

        Handle UnderlyingHandle();

        IProducer<string, string> UnderlyingProducerInstance();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}