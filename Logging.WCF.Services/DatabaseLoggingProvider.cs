using Logging.WCF.Infrastructure.Contracts;
using Logging.WCF.Models.DTOs;
using Logging.WCF.Repository.EF;
using Logging.WCF.Repository.EF.ExtensionMethods;
using Logging.WCF.Repository.EF.Repositories.DomainCore.RepositoryBaseImpl.RepositoryBaseInterfaces;
using System;
using System.Threading.Tasks;

namespace Logging.WCF.Services
{
    /// <inheritdoc />
    public class DatabaseLoggingProvider : ILogSinkerService
    {
        public DatabaseLoggingProvider(IEntityBaseRepositoryAsync<DatabaseLog> dbLogRepository)
        {
            _databaseLogRepository = dbLogRepository;
        }

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
        public async Task<int?> LogAsync(LoggingEventDto loggingEventDto)
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
            }

            return retVal;
        }

        public bool Disposed { get; set; }
        public IEntityBaseRepositoryAsync<DatabaseLog> _databaseLogRepository { get; }
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

        public void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     TODO: This should be encrypted due to possible sensitive data being transmitted (for logging purposes)...
        /// </summary>
        /// <param name="loggingEventDto"></param>
        /// <returns></returns>
        public async Task<int?> LogAsync(LoggingEventDto loggingEventDto)
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
                // LogFactory.GetLogger().LogWarning(this, myLog.ExceptionMessage, dbException);
            }

            return retVal;
        }

        public bool Disposed { get; set; }
    }
}