using Confluent.Kafka;
using KafkaModule.Core.Contracts;

namespace KafkaModule.Core.Implementations
{
    public class KafkaProducerTestImplementation : IKafkaScheduledProducer
    {
        private static readonly Random rand = new Random();
        private readonly ProducerConfig _config;
        private readonly IProducer<string, string> _producer;
        private readonly string _topicName;

        public KafkaProducerTestImplementation(ProducerConfig config, string topicName)
        {
            _topicName = topicName;
            _config = config;
            _config.BootstrapServers = "lxdspkfkv1t.dc.ht.hr:9093, lxdspkfkv2t.dc.ht.hr:9093, lxdspkfkv3t.dc.ht.hr:9093";
            _producer = new ProducerBuilder<string, string>(_config).Build();
        }

        public async Task WriteMessageAsync(string message)
        {
            await _producer.ProduceAsync(_topicName,
                    new Message<string, string> { Key = rand.Next(5).ToString(), Value = message })
                .ContinueWith(task => task.IsFaulted
                    ? $"error producing message: {task.Exception.Message}"
                    : $"produced to: {task.Result.TopicPartitionOffset}");

            // block until all in-flight produce requests have completed (successfully
            // or otherwise) or 10s has elapsed.
            _producer.Flush(TimeSpan.FromSeconds(10));
        }

        public void Dispose()
        {
            _producer?.Flush();
            _producer?.Dispose();
        }

        public Handle UnderlyingHandle()
        {
            return _producer.Handle;
        }

        public IProducer<string, string> UnderlyingProducerInstance()
        {
            return _producer;
        }
    }
}