namespace KafkaModule.Core.DTOs
{
    internal class PersistingResult
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public object Partition { get; set; }
        public object Offset { get; set; }
        public string RequestMethod { get; set; }
        public string ResourceType { get; set; }
        public bool IsFatal { get; set; }
    }
}