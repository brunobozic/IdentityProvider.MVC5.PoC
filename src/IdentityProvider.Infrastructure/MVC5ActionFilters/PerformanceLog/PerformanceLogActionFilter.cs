using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.MVC5ActionFilters.PerformanceLog.Extensions;
using IdentityProvider.Infrastructure.MVC5ActionFilters.PerformanceLog.Model;
using IdentityProvider.Infrastructure.MVC5ActionFilters.PerformanceLog.Provider;
using System;
using System.Diagnostics;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IdentityProvider.Infrastructure.MVC5ActionFilters.PerformanceLog
{
    public class PerformanceLogActionFilter : IActionFilter
    {
        #region Ctor

        public PerformanceLogActionFilter(
            IPerformanceLogProvider performanceLogger
            , IApplicationConfiguration applicationConfiguration
        )
        {
            _performanceLogger = performanceLogger;
            _applicationConfiguration = applicationConfiguration;
            _headerKey = "corr";
        }

        #endregion Ctor

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var jsonPostedData = string.Empty;
            var jsonResponseData = string.Empty;

            if (HttpContext.Current == null)
                return;

            _correlationId = GetCorrelationId();
            _headerCorrelationId = GetHeaderCorrelationId();

            if (_applicationConfiguration.SerilogSqlLitePerformanceLogRequestResponseEnabled())
                try
                {
                    jsonPostedData = filterContext.HttpContext.Request.Form.ConvertToJson();
                    // jsonResponseData = filterContext.HttpContext.Response.?.?.ConvertToJson();
                }
                catch (Exception)
                {
                    // ignored
                }

            _performanceLogger?.Log(CreateNewPerformanceLogTick(filterContext, _correlationId, _headerCorrelationId,
                null, jsonPostedData));

            if (filterContext.HttpContext != null)
                filterContext.HttpContext.Items[_stopwatchKey] = Stopwatch.StartNew();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var jsonPostedData = string.Empty;
            var jsonResponseData = string.Empty;

            var sw = filterContext.HttpContext.Items[_stopwatchKey] as Stopwatch;

            if (_applicationConfiguration.SerilogSqlLitePerformanceLogRequestResponseEnabled())
                try
                {
                    jsonPostedData = filterContext.HttpContext.Request.Form.ConvertToJson();
                    // jsonResponseData = filterContext.HttpContext.Response.?.?.ConvertToJson();

                    if (jsonPostedData == "{}")
                        jsonPostedData = string.Empty;
                }
                catch (Exception)
                {
                    // ignored
                }

            _performanceLogger?.Log(CreateNewPerformanceLogTick(filterContext, _correlationId, _headerCorrelationId, sw,
                jsonPostedData, filterContext.Exception));
        }

        private static string GetCorrelationId()
        {
            var returnValue = (string)HttpContext.Current.Items[CorrelationIdItemName];
            if (string.IsNullOrEmpty(returnValue))
            {
                HttpContext.Current.Items[CorrelationIdItemName] = Guid.NewGuid().ToString();
                returnValue = (string)HttpContext.Current.Items[CorrelationIdItemName];
            }

            return returnValue;
        }

        private string GetHeaderCorrelationId()
        {
            var header = HttpContext.Current.Request.Headers[_headerKey];
            var correlationId = string.IsNullOrEmpty(header)
                ? Guid.NewGuid().ToString()
                : header;


            if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                HttpContext.Current.Response.AddHeader(_headerKey, correlationId);

            return correlationId;
        }

        public void CorrelationIdHeaderEnricher(string headerKey)
        {
            _headerKey = headerKey;
        }

        private PerformanceLogTick CreateNewPerformanceLogTick(ActionExecutedContext filterContext,
            string correlationId, string headerCorrelationId, Stopwatch sw, string jsonPostedData,
            Exception exception = null)
        {
            if (filterContext == null) throw new ArgumentNullException(nameof(filterContext));

            return new PerformanceLogTick
            {
                Action = "1",
                Url = WebUtility.UrlDecode(filterContext.HttpContext?.Request.Url?.AbsoluteUri ?? string.Empty),
                RequestJson = jsonPostedData == string.Empty ? null : jsonPostedData,
                Browser = filterContext.HttpContext?.Request.Browser?.Type ?? string.Empty,
                HttpResponseStatusCode = filterContext.HttpContext?.Response.StatusCode.ToString(),
                HttpResponse = filterContext.HttpContext?.Response.StatusDescription ?? string.Empty,
                Stopwatch = sw,
                CorrelationId = GetCorrelationId(),
                CorrelationHeaderId = GetHeaderCorrelationId(),
                Exception = exception
            };
        }

        private PerformanceLogTick CreateNewPerformanceLogTick(ActionExecutingContext filterContext,
            string correlationId, string headerCorrelationId, Stopwatch sw, string jsonPostedData,
            Exception exception = null)
        {
            if (filterContext == null) throw new ArgumentNullException(nameof(filterContext));

            return new PerformanceLogTick
            {
                Action = "0",
                Url = WebUtility.UrlDecode(filterContext.HttpContext?.Request.Url?.AbsoluteUri ?? string.Empty),
                RequestJson = jsonPostedData == string.Empty ? null : jsonPostedData,
                Browser = filterContext.HttpContext?.Request.Browser?.Type ?? string.Empty,
                HttpResponseStatusCode = filterContext.HttpContext?.Response.StatusCode.ToString(),
                HttpResponse = filterContext.HttpContext?.Response.StatusDescription ?? string.Empty,
                Stopwatch = sw,
                CorrelationId = GetCorrelationId(),
                CorrelationHeaderId = GetHeaderCorrelationId(),
                Exception = exception
            };
        }

        #region Private Props

        private static readonly string _stopwatchKey = "PerfLog_Stopwatch";
        private readonly IPerformanceLogProvider _performanceLogger;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private const string CorrelationIdPropertyName = "CorrelationId";

        private static readonly string CorrelationIdItemName =
            $"{typeof(PerformanceLogActionFilter).Name}+CorrelationId";

        private string _headerKey;
        private string _correlationId;
        private string _headerCorrelationId;

        #endregion Private Props
    }
}