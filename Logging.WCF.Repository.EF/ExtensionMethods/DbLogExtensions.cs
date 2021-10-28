using System;
using System.Collections.Generic;
using Logging.WCF.Repository.EF.Repositories.Bussiness.ConcreteRepositories.ConcreteRepositoryInterfaces;

namespace Logging.WCF.Repository.EF.ExtensionMethods
{
    public static class DbLogExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="messageText"></param>
        /// <returns></returns>
        public static List<DatabaseLog> LogEntriesGetAll(
            this IDbLogRepository repository
            , DateTime? dateFrom
            , DateTime? dateTo
            , string messageText
        )
        {
            if (string.IsNullOrEmpty(messageText))
                throw new ArgumentException("MessageText");


            return null;
        }
    }
}