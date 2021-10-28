﻿using System;
using System.Threading.Tasks;
using log4net;
using Logging.WCF.Infrastructure.Contracts;
using Logging.WCF.Models.DTOs;
using Logging.WCF.Repository.EF;
using Logging.WCF.Repository.EF.ExtensionMethods;
using Logging.WCF.Repository.EF.RepositoryBaseImpl.RepositoryBaseInterfaces;

namespace Logging.WCF.Services.AvailableLogSinkers
{
    /// <inheritdoc />
    public class DatabaseSink : ILogSinkerService
    {
        public DatabaseSink(IEntityBaseRepositoryAsync<DatabaseLog> dbLogRepository)
        {
            _databaseLogRepository = dbLogRepository;
        }

        public bool Disposed { get; set; }
        public IEntityBaseRepositoryAsync<DatabaseLog> _databaseLogRepository { get; }

        public void Dispose()
        {
            Disposed = true;
        }

        /// <inheritdoc />
        /// <summary>
        ///     TODO: This should be encrypted due to possible sensitive data being transmitted (for logging purposes)...
        /// </summary>
        /// <param name="loggingEventDto"></param>
        /// <returns></returns>
        public async Task<int?> SinkToLogAsync(LoggingEventDto loggingEventDto)
        {
            int? retVal = -1;

            var myLog = loggingEventDto.ConvertToDbLog();

            try
            {
                _databaseLogRepository.AddLogEntry(myLog);
                await _databaseLogRepository.CommitAsync();
                retVal = 1;
            }
            catch (Exception dbException)
            {
                myLog.ExceptionMessage +=
                    string.Format("		[ DB ] ====>		Db exception while saving exception: [ {0} ]   [ {1} ]",
                        dbException.Message, dbException.InnerException);

                // LogToWCF to file...
                var logger = LogManager.GetLogger(GetType());
                logger.Fatal("", dbException);
            }

            return retVal;
        }
    }

    public class FakeDbLogService : ILogSinkerService
    {
        private readonly IEntityBaseRepositoryAsync<DatabaseLog> _databaseLogRepository;

        public FakeDbLogService()
        {
        }

        public FakeDbLogService(IEntityBaseRepositoryAsync<DatabaseLog> dbLogRepository)
        {
            _databaseLogRepository = dbLogRepository;
        }

        public bool Disposed { get; set; }

        public void Dispose()
        {
            if (!Disposed) Disposed = true;
        }

        /// <inheritdoc />
        /// <summary>
        ///     TODO: This should be encrypted due to possible sensitive data being transmitted (for logging purposes)...
        /// </summary>
        /// <param name="loggingEventDto"></param>
        /// <returns></returns>
        public async Task<int?> SinkToLogAsync(LoggingEventDto loggingEventDto)
        {
            int? retVal = -1;

            var myLog = loggingEventDto.ConvertToDbLog();

            try
            {
                _databaseLogRepository.AddLogEntry(myLog);
                await _databaseLogRepository.CommitAsync();
                retVal = 1;
            }
            catch (Exception dbException)
            {
                myLog.ExceptionMessage +=
                    string.Format("		[ DB ] ====>		Db exception while saving exception: [ {0} ]   [ {1} ]",
                        dbException.Message, dbException.InnerException);

                // LogToWCF to file... is not relevant to this test
                var logger = LogManager.GetLogger(GetType());
                logger.Fatal(dbException);
            }

            return retVal;
        }
    }
}