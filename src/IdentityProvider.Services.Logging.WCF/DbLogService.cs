using System;
using IdentityProvider.Infrastructure.DatabaseLog;
using IdentityProvider.Infrastructure.DatabaseLog.DTOs;
using IdentityProvider.Infrastructure.DatabaseLog.Model;
using IdentityProvider.Infrastructure.DatabaseLog.Model.ExtensionMethods;
using IdentityProvider.Infrastructure.Logging.Log4Net;
using IdentityProvider.Repository.EF.Factories;
using Module.Repository.EF;
using Module.Repository.EF.UnitOfWorkInterfaces;

namespace HAC.Helpdesk.Services.Logging.WCF
{
    /// <inheritdoc />
    public class DbLogService : IDbLogService
    {
        private readonly IUnitOfWorkAsync _uow;

        public DbLogService(IUnitOfWorkAsync uow)
        {
            _uow = uow;
        }

        public DbLogService()
        {
            if (_uow == null)
            {
                var ctx = DataContextFactory.GetDataContextAsync();

                _uow = new UnitOfWork(ctx);
            }
        }

        public bool Disposed { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     TODO: This should be encrypted due to possible sensitive data being transmitted (for logging purposes)...
        /// </summary>
        /// <param name="loggingEventDto"></param>
        /// <returns></returns>
        public int? LogToDatabase(LoggingEventDto loggingEventDto)
        {
            int? retVal = -1;

            var myLog = loggingEventDto.ConvertToDbLog();

            try
            {
                _uow.RepositoryAsync<DbLog>().Insert(myLog);
                _uow.SaveChangesAsync();
                retVal = 1;
            }
            catch (Exception dbException)
            {
                myLog.ExceptionMessage +=
                    string.Format("		[ DB ] ====>		Db exception while saving exception: [ {0} ]   [ {1} ]",
                        dbException.Message, dbException.InnerException);

                // Log to file...
                Log4NetLoggingFactory.GetLogger().LogWarning(this, myLog.ExceptionMessage, dbException, false);
            }

            return retVal;
        }

        public void Dispose()
        {
            if (_uow != null)
            {
                try
                {
                  
                }
                catch (Exception disposeException)
                {
                    Log4NetLoggingFactory.GetLogger().LogWarning(this, "IDisposable - problem disposing", disposeException);
                }
            }

            Disposed = true;
        }
    }

    public class FakeDbLogService : IDbLogService
    {
        private readonly IUnitOfWorkAsync _uow;

        public FakeDbLogService(IUnitOfWorkAsync uow)
        {
            _uow = uow;
        }

        public FakeDbLogService()
        {
            // _uow = new SimpleMembershipUnitOfWorkAsync(DataContextFactory.GetSimpleMembershipDataContextAsync());
        }

        public bool Disposed { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     TODO: This should be encrypted due to possible sensitive data being transmitted (for logging purposes)...
        /// </summary>
        /// <param name="loggingEventDto"></param>
        /// <returns></returns>
        public int? LogToDatabase(LoggingEventDto loggingEventDto)
        {
            int? retVal = -1;

            var myLog = loggingEventDto.ConvertToDbLog();

            try
            {
                _uow.RepositoryAsync<DbLog>().Insert(myLog);
                _uow.SaveChangesAsync();
                retVal = 1;
            }
            catch (Exception dbException)
            {
                myLog.ExceptionMessage +=
                   string.Format("		[ DB ] ====>		Db exception while saving exception: [ {0} ]   [ {1} ]",
                       dbException.Message, dbException.InnerException);

                // Log to file... is not relevant to this test
                // LogFactory.GetLogger().LogWarning(this, myLog.ExceptionMessage, dbException);
            }

            return retVal;
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                // release any unmanaged objects
                // set object references to null
                if (_uow != null)
                {
                    try
                    {
                        
                    }
                    catch (Exception)
                    {
                        // Log to file... is not relevant to this test
                        // LogFactory.GetLogger().LogWarning(this, "IDisposable - problem disposing", disposeException);
                    }
                }

                Disposed = true;
            }
        }
    }
}