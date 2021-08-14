using IdentityProvider.Infrastructure.Extensions;
using IdentityProvider.Infrastructure.Logging.Serilog.SQLite.Sinks.Batch;
using IdentityProvider.Infrastructure.Logging.Serilog.SQLite.Sinks.Extensions;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;

namespace IdentityProvider.Infrastructure.Logging.Serilog.SQLite.Sinks.SQLite
{
    internal class SQLiteSinkPerformanceLog : BatchProvider, ILogEventSink
    {
        private readonly string _connString;
        private readonly IFormatProvider _formatProvider;
        private readonly TimeSpan? _retentionPeriod;
        private readonly Stopwatch _retentionWatch = new Stopwatch();
        private readonly bool _storeTimestampInUtc;
        private readonly string _tableName;

        public SQLiteSinkPerformanceLog(string sqlLiteDbPath,
            string tableName,
            IFormatProvider formatProvider,
            bool storeTimestampInUtc,
            TimeSpan? retentionPeriod)
        {
            _connString = CreateConnectionString(sqlLiteDbPath);
            _tableName = tableName;
            _formatProvider = formatProvider;
            _storeTimestampInUtc = storeTimestampInUtc;

            if (retentionPeriod.HasValue)
                // impose a min retention period of 1 minute
                _retentionPeriod = new[] { retentionPeriod.Value, TimeSpan.FromMinutes(1) }.Max();

            InitializeDatabase();
        }

        #region ILogEvent implementation

        public void Emit(LogEvent logEvent)
        {
            PushEvent(logEvent);
        }

        #endregion

        private static string CreateConnectionString(string dbPath)
        {
            return new SQLiteConnectionStringBuilder { DataSource = dbPath }.ConnectionString;
        }

        private void InitializeDatabase()
        {
            using (var conn = GetSqLiteConnection())
            {
                CreateSqlTable(conn);
            }
        }

        private System.Data.SQLite.SQLiteConnection GetSqLiteConnection()
        {
            var sqlConnection = new SQLiteConnection(_connString);
            sqlConnection.Open();
            return sqlConnection;
        }

        private void CreateSqlTable(SQLiteConnection sqlConnection)
        {
            // {CurrentEnvironment}{ApplicationId}{InstanceId}{OIB}{NameId}{SessionIndex}{Action}{Url}{Status}{StatusCode}{Browser}{Request}{Response}{Miliseconds}

            var colDefs = "id INTEGER PRIMARY KEY AUTOINCREMENT,";
            colDefs += "Timestamp INT,";
            colDefs += "ApplicationId TEXT,";
            colDefs += "InstanceId TEXT,";
            colDefs += "Environment TEXT,";
            colDefs += "OIB TEXT,";
            colDefs += "SubjectId TEXT,";
            colDefs += "SessionIndex TEXT,";
            colDefs += "Url TEXT,";
            colDefs += "Action TEXT,";
            colDefs += "Request TEXT,";
            colDefs += "Response TEXT,";
            colDefs += "Status TEXT,";
            colDefs += "StatusCode TEXT,";
            colDefs += "Browser TEXT,";
            colDefs += "Miliseconds DOUBLE,";
            colDefs += "Level VARCHAR(10),";
            colDefs += "Exception TEXT,";
            colDefs += "RenderedMessage TEXT,";
            colDefs += "Properties TEXT,";
            colDefs += "CorrelationId TEXT";

            var sqlCreateText = $"CREATE TABLE IF NOT EXISTS {_tableName} ({colDefs})";

            var sqlCommand = new SQLiteCommand(sqlCreateText, sqlConnection);
            sqlCommand.ExecuteNonQuery();
        }

        private SQLiteCommand CreateSqlInsertCommand(SQLiteConnection connection)
        {
            var sqlInsertText =
                "INSERT INTO {0} (Timestamp, ApplicationId, InstanceId, Environment, OIB, SubjectId, SessionIndex, Url, Action, Request, Response, Status, StatusCode, Browser, Miliseconds, Level, Exception, RenderedMessage, CorrelationId, Properties)";
            sqlInsertText +=
                " VALUES (@timeStamp, @applicationId, @instanceId, @environment, @oib, @subjectId, @sessionIndex, @url, @action, @request, @response, @status, @statusCode, @browser, @miliseconds, @level, @exception, @renderedMessage, @CorrelationId, @properties)";
            sqlInsertText = string.Format(sqlInsertText, _tableName);

            var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = sqlInsertText;
            sqlCommand.CommandType = CommandType.Text;

            sqlCommand.Parameters.Add(new SQLiteParameter("@timeStamp", DbType.DateTime));
            sqlCommand.Parameters.Add(new SQLiteParameter("@applicationId", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@instanceId", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@environment", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@OIB", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@subjectId", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@sessionIndex", DbType.String));

            sqlCommand.Parameters.Add(new SQLiteParameter("@url", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@action", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@request", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@response", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@status", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@statusCode", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@browser", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@miliseconds", DbType.Double));

            sqlCommand.Parameters.Add(new SQLiteParameter("@level", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@exception", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@renderedMessage", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@correlationId", DbType.String));
            sqlCommand.Parameters.Add(new SQLiteParameter("@properties", DbType.String));

            return sqlCommand;
        }

        protected override void WriteLogEvent(ICollection<LogEvent> logEventsBatch)
        {
            if (logEventsBatch == null || logEventsBatch.Count == 0)
                return;
            try
            {
                using (var sqlConnection = GetSqLiteConnection())
                {
                    ApplyRetentionPolicy(sqlConnection);

                    using (var tr = sqlConnection.BeginTransaction())
                    {
                        using (var sqlCommand = CreateSqlInsertCommand(sqlConnection))
                        {
                            sqlCommand.Transaction = tr;

                            foreach (var logEvent in logEventsBatch)
                            {
                                sqlCommand.Parameters["@timeStamp"].Value = logEvent.Timestamp.UtcDateTime;

                                sqlCommand.Parameters["@applicationId"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("ApplicationId")).SingleOrDefault()
                                        .Value?.ToString()?.CleanUpSerilogProperties() ?? string.Empty;
                                sqlCommand.Parameters["@instanceId"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("InstanceId")).SingleOrDefault().Value
                                        ?.ToString()?.CleanUpSerilogProperties() ?? string.Empty;
                                sqlCommand.Parameters["@environment"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("CurrentEnvironment"))
                                        .SingleOrDefault().Value?.ToString()?.CleanUpSerilogProperties() ??
                                    string.Empty;
                                sqlCommand.Parameters["@OIB"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("OIB")).SingleOrDefault().Value
                                        ?.ToString()?.CleanUpSerilogProperties() ?? string.Empty;
                                sqlCommand.Parameters["@subjectId"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("NameId")).SingleOrDefault().Value
                                        ?.ToString()?.CleanUpSerilogProperties() ?? string.Empty;
                                sqlCommand.Parameters["@sessionIndex"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("SessionIndex")).SingleOrDefault()
                                        .Value?.ToString()?.CleanUpSerilogProperties() ?? string.Empty;

                                sqlCommand.Parameters["@url"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("Url")).SingleOrDefault().Value
                                        ?.ToString()?.CleanUpSerilogProperties() ?? string.Empty;
                                sqlCommand.Parameters["@action"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("Action")).SingleOrDefault().Value
                                        ?.ToString()?.CleanUpSerilogProperties() ?? string.Empty;
                                sqlCommand.Parameters["@request"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("Request")).SingleOrDefault().Value
                                        ?.ToString() ?? string.Empty;
                                sqlCommand.Parameters["@response"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("Response")).SingleOrDefault().Value
                                        ?.ToString() ?? string.Empty;
                                sqlCommand.Parameters["@status"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("Status")).SingleOrDefault().Value
                                        ?.ToString().CleanUpSerilogProperties() ?? string.Empty;
                                sqlCommand.Parameters["@statusCode"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("StatusCode")).SingleOrDefault().Value
                                        ?.ToString().CleanUpSerilogProperties() ?? string.Empty;
                                sqlCommand.Parameters["@browser"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("Browser")).SingleOrDefault().Value
                                        ?.ToString().CleanUpSerilogProperties() ?? string.Empty;
                                sqlCommand.Parameters["@miliseconds"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("Miliseconds")).SingleOrDefault().Value
                                        ?.ToString()?.CleanUpSerilogProperties() ?? string.Empty;

                                sqlCommand.Parameters["@level"].Value = logEvent.Level.ToString();

                                sqlCommand.Parameters["@exception"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("Exception")).SingleOrDefault().Value
                                        ?.ToString() ?? string.Empty;
                                sqlCommand.Parameters["@exception"].Value +=
                                    logEvent.Exception?.ToString() ?? string.Empty;

                                sqlCommand.Parameters["@renderedMessage"].Value = logEvent.MessageTemplate.ToString();

                                sqlCommand.Parameters["@correlationId"].Value =
                                    logEvent.Properties?.Where(i => i.Key.Equals("CorrelationId")).SingleOrDefault()
                                        .Value?.ToString().CleanUpSerilogProperties() ?? string.Empty;


                                sqlCommand.Parameters["@properties"].Value = logEvent.Properties.Count > 0
                                    ? logEvent.Properties.Json()
                                    : string.Empty;

                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                        tr.Commit();
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                SelfLog.WriteLine(e.Message);
                if (e.Message.Contains("database is locked"))
                    throw new Exception("Database is locked, no audit logging will be possible!");
            }
        }

        private void ApplyRetentionPolicy(SQLiteConnection sqlConnection)
        {
            if (!_retentionPeriod.HasValue)
                // there is no retention policy
                return;

            if (_retentionWatch.IsRunning && _retentionWatch.Elapsed < _retentionPeriod.Value)
                // Besides deleting records older than X 
                // let's only delete records every X often
                // because of the check whether the _retentionWatch is running,
                // the first write operation during this application run
                // will result in deleting old records
                return;

            var epoch = DateTimeOffset.Now.Subtract(_retentionPeriod.Value);
            using (var cmd = CreateSqlDeleteCommand(sqlConnection, epoch))
            {
                SelfLog.WriteLine("Deleting log entries older than {0}", epoch);
                cmd.ExecuteNonQuery();
            }

            _retentionWatch.Restart();
        }

        private SQLiteCommand CreateSqlDeleteCommand(SQLiteConnection sqlConnection, DateTimeOffset epoch)
        {
            var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = $"DELETE FROM {_tableName} WHERE Timestamp < @epoch";
            cmd.Parameters.Add(new SQLiteParameter("@epoch", DbType.DateTime2)
            {
                Value = _storeTimestampInUtc ? epoch.ToUniversalTime() : epoch
            });
            return cmd;
        }
    }
}