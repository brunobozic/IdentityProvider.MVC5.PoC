﻿namespace KafkaModule.Core.DTOs
{
    public class KafkaProducerRetryPolicy
    {
        public int RetryTimes { get; set; }
        public KafkaSharedExponentialBackoff ExponentialBackoff { get; set; }
    }
}