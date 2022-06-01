using Confluent.Kafka;
using IdentityProvider.Web.MVC6;
using KafkaModule.Core.Contracts;
using KafkaModule.Core.DTOs;
using KafkaModule.Core.Enums;
using KafkaModule.Core.Enums.EnumExtensions;
using Serilog;

namespace KafkaModule.Core.Implementations
{
    public class KafkaLoggingProducer : IKafkaLoggingProducer
    {
        private static readonly Random rand = new Random();
        private readonly ProducerConfig _config;
        private readonly IProducer<string, string> _producer;
        private readonly string _topicName;

        public KafkaLoggingProducer()
        { }

        public KafkaLoggingProducer(MyConfigurationValues settings, string topicName)
        {
            _topicName = topicName;

            _config = new ProducerConfig
            {
                BootstrapServers = settings.KafkaLoggingProducerSettings.BootstrapServers,
                SaslUsername = settings.KafkaLoggingProducerSettings.SaslUsername,
                SaslPassword = settings.KafkaLoggingProducerSettings.SaslPassword,
                SslCaLocation = settings.KafkaLoggingProducerSettings.CALocation,
                MaxInFlight = 1
            };

            if (settings.KafkaLoggingProducerSettings.SecurityProtocol.ToUpper() == SecurityProtocolEnum.SASL_Plaintext.GetDescriptionString())
            {
                _config.SecurityProtocol = SecurityProtocol.SaslPlaintext;
            }

            if (settings.KafkaLoggingProducerSettings.SecurityProtocol.ToUpper() == SecurityProtocolEnum.SASL_ssl.GetDescriptionString())
            {
                _config.SecurityProtocol = SecurityProtocol.SaslSsl;
            }

            if (settings.KafkaLoggingProducerSettings.SaslMechanism.ToUpper() == SaslMechanismEnum.Plain.GetDescriptionString()) { _config.SaslMechanism = SaslMechanism.Plain; }
            if (settings.KafkaLoggingProducerSettings.SaslMechanism.ToUpper() == SaslMechanismEnum.SCRAMSHA256.GetDescriptionString()) { _config.SaslMechanism = SaslMechanism.ScramSha256; }

            if (settings.KafkaLoggingProducerSettings.Debug.HasValue && settings.KafkaLoggingProducerSettings.Debug.Value) { _config.Debug = "ALL"; }

            MessageProducingResult messageProducingResult;

            try
            {
                _producer = new ProducerBuilder<string, string>(_config).Build();
            }
            catch (Exception producingEx)
            {
                Log.Error(
                   "Message producer failed, reason [ " + producingEx.Message + " ].", producingEx);

                messageProducingResult = new MessageProducingResult
                {
                    ProducedStatusMessage = "Message producer failed, reason [ " + producingEx.Message + " ] ",
                    ErrorType = "produce"
                };
            }
        }

        public async Task WriteLogMessageAsync(string message)
        {
            try
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
            catch (Exception loggingEx)
            {
                Console.WriteLine(loggingEx.Message);
            }
        }

        public void Dispose()
        {
            _producer?.Flush();
            _producer?.Dispose();
        }

        public Handle UnderlyingHandle()
        {
            return _producer != null ? _producer.Handle : null;
        }

        public IProducer<string, string> UnderlyingProducerInstance()
        {
            return _producer;
        }
    }
}