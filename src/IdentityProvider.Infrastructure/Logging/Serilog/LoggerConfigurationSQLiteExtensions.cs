using IdentityProvider.Infrastructure.Logging.Serilog.SQLite.Sinks.SQLite;
using Serilog;
using Serilog.Configuration;
using Serilog.Debugging;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;

namespace IdentityProvider.Infrastructure.Logging.Serilog
{
    /// <summary>
    ///     Adds the WriteTo.SQLite() extension method to <see cref="LoggerConfiguration" />.
    /// </summary>
    public static class LoggerConfigurationSQLiteExtensions
    {
        /// <summary>
        ///     Adds a sink that writes log events to a SQLite database.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="sqliteDbPath">The path of SQLite db.</param>
        /// <param name="tableName">The name of the SQLite table to store log.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="storeTimestampInUtc">Store timestamp in UTC format</param>
        /// <param name="retentionPeriod">
        ///     The maximum time that a log entry will be kept in the database, or null to disable
        ///     automatic deletion of old log entries. Non-null values smaller than 1 minute will be replaced with 1 minute.
        /// </param>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration SQLite(
            this LoggerSinkConfiguration loggerConfiguration,
            string sqliteDbPath,
            string tableName = "Logs",
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            bool storeTimestampInUtc = false,
            TimeSpan? retentionPeriod = null)
        {
            if (loggerConfiguration == null)
            {
                SelfLog.WriteLine("Logger configuration is null");
                throw new ArgumentNullException(nameof(loggerConfiguration));
            }

            if (string.IsNullOrEmpty(sqliteDbPath))
            {
                SelfLog.WriteLine("Invalid sqliteDbPath");
                throw new ArgumentNullException(nameof(sqliteDbPath));
            }

            Uri sqliteDbPathUri;
            if (!Uri.TryCreate(sqliteDbPath, UriKind.RelativeOrAbsolute, out sqliteDbPathUri))
                throw new ArgumentException($"Invalid path {nameof(sqliteDbPath)}");

            if (!sqliteDbPathUri.IsAbsoluteUri)
            {
                var basePath = Assembly.GetEntryAssembly().Location;
                sqliteDbPath = Path.Combine(Path.GetDirectoryName(basePath), sqliteDbPath);
            }

            try
            {
                var sqliteDbFile = new FileInfo(sqliteDbPath);
                sqliteDbFile.Directory?.Create();

                return loggerConfiguration.Sink(
                    new SQLiteSink(
                        sqliteDbFile.FullName,
                        tableName,
                        formatProvider,
                        storeTimestampInUtc,
                        retentionPeriod),
                    restrictedToMinimumLevel);
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine(ex.Message);
                throw;
            }
        }

        public static LoggerConfiguration SQLitePerformanceAudit(
            this LoggerSinkConfiguration loggerConfiguration,
            string sqliteDbPath,
            string tableName = "PerformanceLog",
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null,
            bool storeTimestampInUtc = false,
            TimeSpan? retentionPeriod = null)
        {
            if (loggerConfiguration == null)
            {
                SelfLog.WriteLine("Logger configuration is null");
                throw new ArgumentNullException(nameof(loggerConfiguration));
            }

            if (string.IsNullOrEmpty(sqliteDbPath))
            {
                SelfLog.WriteLine("Invalid sqliteDbPath");
                throw new ArgumentNullException(nameof(sqliteDbPath));
            }

            Uri sqliteDbPathUri;
            if (!Uri.TryCreate(sqliteDbPath, UriKind.RelativeOrAbsolute, out sqliteDbPathUri))
                throw new ArgumentException($"Invalid path {nameof(sqliteDbPath)}");

            if (!sqliteDbPathUri.IsAbsoluteUri)
            {
                var basePath = Assembly.GetEntryAssembly().Location;
                sqliteDbPath = Path.Combine(Path.GetDirectoryName(basePath), sqliteDbPath);
            }

            try
            {
                var sqliteDbFile = new FileInfo(sqliteDbPath);
                sqliteDbFile.Directory?.Create();

                return loggerConfiguration.Sink(
                    new SQLiteSinkPerformanceLog(
                        sqliteDbFile.FullName,
                        tableName,
                        formatProvider,
                        storeTimestampInUtc,
                        retentionPeriod),
                    restrictedToMinimumLevel);
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine(ex.Message);
                throw;
            }
        }
    }
}