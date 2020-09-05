using Module.Repository.EF.Repositories;
using System;
using System.Collections.Generic;

namespace IdentityProvider.Repository.EF.Repositories.DbLog.Extensions
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
        public static List<Infrastructure.DatabaseLog.Model.DbLog> LogEntriesGetAll(
            this IRepository<Infrastructure.DatabaseLog.Model.DbLog> repository
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