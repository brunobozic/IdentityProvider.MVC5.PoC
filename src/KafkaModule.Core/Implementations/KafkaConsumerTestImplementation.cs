using Confluent.Kafka;
using KafkaModule.Core.Contracts;
using KafkaModule.Core.DTOs;
using Serilog;
using System.Text;

namespace KafkaModule.Core.Implementations
{
    public class KafkaConsumerTestImplementation : IKafkaScheduledConsumer
    {
        #region Public Properties

        public IConsumer<string, string> Instance()
        {
            return _c;
        }

        public readonly TopicPartitionOffset CurrentTopicPartitionOffset;

        #endregion Public Properties

        #region Private Props

        private readonly string _topicName;
        private readonly IConsumer<string, string> _c;
        private ConsumeResult<string, string> _consumedMessage;

        // ReSharper disable once NotAccessedField.Local
        private ConsumerConfig _config;

        private long _currentOffset { get; set; }
        private long _lastOffset { get; set; }
        private long _needsToHitOffset { get; set; }
        private TopicPartition _needsToHitTopicPartition { get; set; }

        #endregion Private Props

        public KafkaConsumerTestImplementation(ConsumerConfig config, string topicName)
        {
            if (string.IsNullOrEmpty(topicName)) throw new ArgumentNullException(nameof(topicName));

            _config = config ?? throw new ArgumentNullException(nameof(config));
            _topicName = topicName;

            _config = new ConsumerConfig
            {
                BootstrapServers = config.BootstrapServers,
                // SslCaLocation = "./Certificates/idp.test.client/dsptest-root-ca.pem",
                // SslCertificateLocation = "./Certificates/idp.test.client/donat-gadm.test.client-ca1-signed.crt",
                SecurityProtocol = SecurityProtocol.Plaintext,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "admin",
                SaslPassword = "admin012",
                GroupId = "CG_TEST_IDP-IDP_ADDRESS-CHANGE-EVENTS",
                Debug = "consumer,topic",
                ClientId = "test_idp",
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnablePartitionEof = config.EnablePartitionEof,
                EnableAutoOffsetStore = config.EnableAutoOffsetStore,
                //MetadataRequestTimeoutMs = config.MetadataRequestTimeoutMs,
                // SslKeyPem = "==",
                // SslCertificatePem = "./Certificates/idp.test.client/donat-gadm.test.client.certificate.pem",

                MaxPollIntervalMs = config.MaxPollIntervalMs
                // ======> TODO: ======> increase maximum poll interval!!!!!!!
            };

            _c = new ConsumerBuilder<string, string>(_config)
                        .SetKeyDeserializer(Deserializers.Utf8)
                        .SetValueDeserializer(Deserializers.Utf8)
                        .Build();

            _c.Subscribe(topicName);
        }

        #region IDisposable

        public void Dispose()
        {
            if (_c == null) return;
            _c.Unsubscribe();
            _c.Close();
            _c.Dispose();
        }

        #endregion IDisposable

        public ConsumeMessageResult Consume()
        {
            var messageConsumingResult = new ConsumeMessageResult();

            try
            {
                _consumedMessage = _c.Consume(3000);

                if (_consumedMessage != null)
                {
                    if (_consumedMessage.IsPartitionEOF)
                    {
                        messageConsumingResult.Message = "IsPartitionEOF";
                        messageConsumingResult.ErrorMessage = "IsPartitionEOF";
                    }
                    else if (_consumedMessage.Message != null)
                    {
                        messageConsumingResult.Success = true;
                        //messageConsumingResult.CurrentTopicPartitionOffset = _consumedMessage.TopicPartitionOffset;
                        messageConsumingResult.CurrentPartition = _consumedMessage.TopicPartitionOffset.Partition.Value;
                        messageConsumingResult.CurrentOffset = _consumedMessage.TopicPartitionOffset.Offset.Value;
                        messageConsumingResult.LowOffset = _c.GetWatermarkOffsets(_consumedMessage.TopicPartition).Low.Value;
                        messageConsumingResult.HiOffset = _c.GetWatermarkOffsets(_consumedMessage.TopicPartition).High.Value;
                        messageConsumingResult.Message = _consumedMessage?.Message?.Value;
                        // messageConsumingResult.MessageId = _consumedMessage.Message?.Key;
                        messageConsumingResult.Offset = _consumedMessage.Offset.Value;
                        messageConsumingResult.Partition = _consumedMessage.Partition.Value;
                        messageConsumingResult.Topic = _consumedMessage.Topic;
                    }

                    // this is the last successfully read offset, that at this point is still not commited
                    // IMPORTANT => last commited offset may or may not be equal to the last offset read from the topic/partition
                    _lastOffset = _consumedMessage.Offset.Value;
                }
                else
                {
                    messageConsumingResult.Message = "No messages were returned from Kafka, broker down?";
                    messageConsumingResult.ErrorMessage = "No messages were returned from Kafka, broker down?";
                }
            }
            catch (ConsumeException cex)
            {
                if (cex.Message.Contains("No offset stored"))
                {
                    Log.Error("Message consumer failed, reason [ " + cex.Message + " ]", cex);

                    messageConsumingResult = new ConsumeMessageResult
                    {
                        ErrorMessage = "Message consumer failed, reason [ " + cex.Message + " ]",
                        ErrorType = "nooffset"
                    };
                }
                else
                {
                    Log.Error("Message consumer failed, reason [ " + cex.Message + " ], offset [ " + _currentOffset + " ]", cex);

                    messageConsumingResult = new ConsumeMessageResult
                    {
                        ErrorMessage = "Message consumer failed, reason [ " + cex.Message + " ], offset [ " + _currentOffset + " ]",
                        ErrorType = "consume",
                        // this is important - we need to read the last offset read and store into a variable
                        Offset = _lastOffset
                    };
                }
            }
            catch (Exception genericException)
            {
                Log.Logger.Error(genericException.Message, genericException);

                messageConsumingResult = new ConsumeMessageResult
                {
                    ErrorMessage = "Message consumer failed, reason [ " + genericException.Message + " ], offset [ " +
                                        _currentOffset + " ]",
                    ErrorType = "general",
                    // this is important - we need to read the last offset read and store into a variable
                    Offset = _lastOffset
                };
            }

            return messageConsumingResult;
        }

        /// <summary>
        /// Returns the underlying consumer instance.
        /// </summary>
        /// <returns></returns>
        public Handle UnderlyingHandle()
        {
            return _c != null ? _c.Handle : null;
        }

        /// <summary>
        /// Returns a list of consumer subscriptions.
        /// </summary>
        /// <returns></returns>
        public List<string> UnderlyingSubscriptions()
        {
            return _c != null ? _c.Subscription : null;
        }

        public void Pause()
        {
            // _c.Pause();
        }

        public void Continue()
        {
            // _c.Resume(?);
        }

        /// <summary>
        /// Returns the topic we have subscribed our consumer to.
        /// </summary>
        /// <returns></returns>
        public string GetTopic()
        {
            return _topicName;
        }

        /// <summary>
        /// Returns the latest offset that we wish to track.
        /// The returned value will not be equal to last commited offset nor the current consumer tracked offset
        /// Instead this will return the last offset we declared as our target offset.
        /// This is due to enabling multiple consecutive message skipping. No logging.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentOffset()
        {
            StringBuilder sb = new StringBuilder();

            if (_c.Assignment.Count > 0)
            {
                int counter = 0;
                sb.AppendFormat("P/Offset: ");
                foreach (var item in _c.Assignment)
                {
                    var offset = _c.Position(new TopicPartition(item.Topic, item.Partition));
                    if (offset.Value != -1001)
                    {
                        sb.AppendFormat("[{0}{1}", item.Partition.Value, "/");
                        sb.AppendFormat("{0}]", offset.Value);
                        if (counter != _c.Assignment.Count - 1)
                            sb.AppendFormat("{0}", ",");
                    }

                    counter = counter + 1;
                }
            }

            return sb.ToString();
        }

        public bool SkipPoisonPill(ConsumeResult<string, string> consumedMessage)
        {
            if (consumedMessage != null)
            {
                var o = new Offset(consumedMessage.Offset.Value + 1);
                var tpo = new TopicPartitionOffset(consumedMessage.TopicPartition, o);

                _c.Seek(tpo);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Skips a message by seeking to one position ahead.
        /// Sets the needsToHitOffset to the targeted position to enable multiple consecutive message skipping.
        /// Handles its own logging.
        /// Returns false if fails.
        /// </summary>
        /// <param name="topicPartition"></param>
        /// <param name="recordOffset"></param>
        /// <returns></returns>
        public bool SkipPoisonPill(TopicPartition topicPartition, long recordOffset)
        {
            Log.Warning("Message at offset: [ {KafkaOffset} ] is to be skipped, brokers: [ {KafkaConsumerBootstrapServer} ]");

            if (string.IsNullOrEmpty(topicPartition.Topic))
            {
                Log.Fatal("Message at offset: [ {KafkaOffset} ] was **not** skipped, brokers: [ {KafkaConsumerBootstrapServer} ], the topic was not defined.");
                return false;
            }

            // we need to track what offset was last stored, what offset we are currently at, and - what offset we need to hit
            // each needs to be tracked separately
            // in this case we are setting the offset that we would like to hit as _needsToHitOffset
            // this is needed because we might be skipping more than 1 consecutive message, and in this case we must not track
            // last commited offset nor current offset but we need to make sure we are tracking the _needsToHitOffset
            _needsToHitOffset = _lastOffset + 1;
            _needsToHitTopicPartition = topicPartition;

            var targetOffset = new TopicPartitionOffset(_needsToHitTopicPartition, _needsToHitOffset);

            try
            {
                _c.Seek(targetOffset);
            }
            catch (Exception ex)
            {
                Log.Fatal("Message at offset: [ {KafkaOffset} ] was **not** skipped, brokers: [ {KafkaConsumerBootstrapServer} ]", ex);

                return false;
            }

            return true;
        }

        public void StoreOffsetFor(ConsumeResult<string, string> msg)
        {
            _c.StoreOffset(msg);

            Log.Information("Message of offset: [ {KafkaOffset} ] offset stored");
        }

        public void Seek(TopicPartition topicPartition, long recordOffset)
        {
            var offset = new Offset(recordOffset);
            var tpo = new TopicPartitionOffset(topicPartition, offset);
            _c.Seek(tpo);
        }

        public string GetBootstrapServers()
        {
            return _config.BootstrapServers;
        }

        public string GetKafkaConsumerMaxOffset()
        {
            StringBuilder sb = new StringBuilder();

            if (_c.Assignment.Count > 0)
            {
                int counter = 0;
                sb.AppendFormat("Part/MaxOffset: ");
                foreach (var item in _c.Assignment)
                {
                    WatermarkOffsets offset = _c.GetWatermarkOffsets(_c.Assignment[counter]);

                    if (offset.High != -1001)
                    {
                        sb.AppendFormat("[{0}{1}", item.Partition.Value, "/");
                        sb.AppendFormat("{0}]", offset.High);
                        if (counter != _c.Assignment.Count - 1)
                            sb.AppendFormat("{0}", ",");
                    }

                    counter += 1;
                }
            }

            return sb.ToString();
        }

        private static ConsumeMessageResult PopulateReturnMessage(ConsumeMessageResult thusFar, string message)
        {
            thusFar.Message = "IsPartitionEOF";
            thusFar.ErrorMessage = "IsPartitionEOF";

            return thusFar;
        }

        private void PopulateTheReturnValue(IConsumer<string, string> consumer, ConsumeResult<string, string> consumedMessage, ConsumeMessageResult returnValue)
        {
            returnValue.Success = true;
            //returnValue.CurrentTopicPartitionOffset = consumedMessage.TopicPartitionOffset;
            returnValue.CurrentPartition = consumedMessage.TopicPartitionOffset.Partition.Value;
            returnValue.CurrentOffset = consumedMessage.TopicPartitionOffset.Offset.Value;
            returnValue.LowOffset = consumer.GetWatermarkOffsets(consumedMessage.TopicPartition).Low.Value;
            returnValue.HiOffset = consumer.GetWatermarkOffsets(consumedMessage.TopicPartition).High.Value;
            returnValue.Message = consumedMessage?.Message?.Value;
            if (_needsToHitOffset == 0)
            {
                returnValue.Offset = consumedMessage.Offset.Value;
            }
            else
            {
                returnValue.Offset = _needsToHitOffset;
            }
            returnValue.Partition = consumedMessage.Partition.Value;
            returnValue.Topic = consumedMessage.Topic;
        }

        private static void ParseAndLog(ConsumeMessageResult messageConsumingResult)
        {
            // here we want to truncate the message so as to avoid flooding the log
            var msgLength = messageConsumingResult.Message?.Length;
            var actualMsg = string.Empty;
            if (msgLength > 20) { actualMsg = messageConsumingResult.Message.Substring(0, 20); } else { actualMsg = messageConsumingResult.Message; }

            Log.Information("Message payload: [ " + actualMsg + " ] at offset: [ {KafkaOffset} ], brokers: [ {KafkaConsumerBootstrapServer} ]");
        }
    }
}