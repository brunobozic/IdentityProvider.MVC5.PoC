using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Events;

namespace IdentityProvider.Infrastructure.Logging.Serilog.SQLite.Sinks.Batch
{
    public static class LogEventExtension
    {
        public static string ExtractPropertyValueByName(this LogEvent logEvent, string parentPropertyName,
            string subPropertyName)
        {
            var returnValue = string.Empty;

            IReadOnlyList<LogEventProperty> myListOfStructuredProperties = new List<LogEventProperty>();
            try
            {
                if (logEvent != null && logEvent.Properties != null)
                    if (logEvent.Properties.Values as StructureValue != null)
                        foreach (StructureValue value in logEvent.Properties.Values)
                            if (value != null && value.TypeTag.Equals(parentPropertyName))
                                myListOfStructuredProperties = value.Properties;

                if (myListOfStructuredProperties.Count > 0)
                    returnValue = myListOfStructuredProperties?.Where(i => i.Name.ToUpper().Equals(subPropertyName))
                        .Select(i => i.Value).Single()?.ToString() ?? string.Empty;
            }
            catch (Exception)
            {
            }

            return returnValue;
        }
    }
}