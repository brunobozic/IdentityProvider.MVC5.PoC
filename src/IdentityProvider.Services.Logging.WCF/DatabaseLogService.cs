
using Logging.WCF.Infrastructure.Contracts;
using Logging.WCF.Models.DTOs;
using Logging.WCF.Repository.EF;
using Logging.WCF.Repository.EF.ExtensionMethods;
using Logging.WCF.Repository.EF.Repositories.DomainCore.RepositoryBaseImpl.RepositoryBaseInterfaces;
using Module.Repository.EF.UnitOfWorkInterfaces;
using System;
using System.Threading.Tasks;

namespace HAC.Helpdesk.Services.Logging.WCF
{
    /// <inheritdoc />
    public class DatabaseLogService : ILogSinkerService
    {
        public DatabaseLogService(IEntityBaseRepositoryAsync<DatabaseLog> dbLogRepository)
        {
            _dbLogRepository = dbLogRepository;
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
        public async System.Threading.Tasks.Task<int?> LogAsync(LoggingEventDto loggingEventDto)
        {
            int? retVal = -1;

            var myLog = loggingEventDto.ConvertToDbLog();

            try
            {
                _dbLogRepository.AddLogEntry(myLog);
                await _dbLogRepository.CommitAsync();
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
        public IEntityBaseRepositoryAsync<DatabaseLog> _dbLogRepository { get; }
    }

    public class FakeDbLogService : ILogSinkerService
    {
        private readonly IUnitOfWorkAsync _uow;

        public FakeDbLogService(IEntityBaseRepositoryAsync<DatabaseLog> dbLogRepository)
        {
            _dbLogRepository = dbLogRepository;
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
                _dbLogRepository.AddLogEntry(myLog);
                _dbLogRepository.CommitAsync();
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
        public IEntityBaseRepositoryAsync<DatabaseLog> _dbLogRepository { get; }
    }
}