using KafkaModule.Core.Contracts;
using Serilog.Core;
using Serilog.Events;
using System.Runtime.CompilerServices;

namespace KafkaModule.Core.Logging.Enrichers
{
    public class KafkaConsumerTopicEnricher : ILogEventEnricher
    {
        private LogEventProperty _cachedProperty;

        public const string PropertyName = "KafkaConsumerTopic";

        public IKafkaScheduledConsumer _consumer { get; set; }
        public IServiceProvider _serviceProvider { get; }

        public KafkaConsumerTopicEnricher()
        {
        }

        public KafkaConsumerTopicEnricher(IKafkaScheduledConsumer consumer, IServiceProvider serviceProvider)
        {
            _consumer = consumer; _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Enrich the log event.
        /// </summary>
        /// <param name="logEvent">The log event to enrich.</param>
        /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
        }

        private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
        {
            // Don't care about thread-safety, in the worst case the field gets overwritten and one property will be GCed
            if (_consumer == null)
            {
                _consumer = (IKafkaScheduledConsumer)_serviceProvider.GetService(typeof(IKafkaScheduledConsumer));
            }

            if (_cachedProperty == null)
                _cachedProperty = CreateProperty(propertyFactory, _consumer);

            return _cachedProperty;
        }

        // Qualify as uncommon-path
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory, IKafkaScheduledConsumer consumer)
        {
            var value = consumer.GetTopic();
            return propertyFactory.CreateProperty(PropertyName, value);
        }
    }
}