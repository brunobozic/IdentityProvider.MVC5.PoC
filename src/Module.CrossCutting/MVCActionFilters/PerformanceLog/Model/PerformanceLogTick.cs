using System.Diagnostics;

namespace Module.CrossCutting.MVCActionFilters.PerformanceLog.Model
{
    public class PerformanceLogTick
    {
        public string Url { get; set; }
        public double Miliseconds { get; set; }
        public string Action { get; set; }
        public Stopwatch Stopwatch { get; set; }
        public string RequestJson { get; set; }
        public string Browser { get; set; }
        public string HttpResponseStatusCode { get; set; }
        public string HttpResponse { get; set; }
        public string ResponseJson { get; set; }
        public string CorrelationId { get; set; }
        public string CorrelationHeaderId { get; set; }
        public Exception Exception { get; set; }
    }
}