using IdentityProvider.Infrastructure.DatabaseLog.Model;
using Module.Repository.EF;
using System;
using System.Linq.Expressions;

namespace IdentityProvider.Repository.EF.Queries.DatabaseLog
{
    public class DatabaseLogQuery : QueryObject<DbLog>
    {
        public DateTime? FromModifiedDate { get; set; }
        public DateTime? ToModifiedDate { get; set; }
        public DateTime? FromCreatedDate { get; set; }
        public DateTime? ToCreatedDate { get; set; }
        public string NessageText { get; set; }
        public string ErrorMessageText { get; set; }
        public int? UserId { get; set; }
        public Guid? TrackingNo { get; set; }
        public int? ErrorLevel { get; set; }
        public string FileName { get; set; }
        public int? LineNo { get; set; }
        public string Method { get; set; }
        public string MethodName { get; set; }
        public string Operation { get; set; }
        public string ControllerName { get; set; }
        public string Url { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


        public override Expression<Func<DbLog, bool>> Query()
        {
            var locFrom = FromModifiedDate.GetValueOrDefault(DateTime.Now.AddDays(-1));
            var locTo = ToModifiedDate.GetValueOrDefault(DateTime.Now);

            if (string.IsNullOrEmpty(NessageText.Trim()))
                return x =>
                    x.ModifiedDate >= locFrom &&
                    x.ModifiedDate <= locTo;

            return x =>
                x.ModifiedDate >= locFrom &&
                x.ModifiedDate <= locTo &&
                x.Message == NessageText;
        }

        /// <summary>
        /// </summary>
        /// <param name="stringToMatch"></param>
        /// <returns></returns>
        public DatabaseLogQuery WhereErrorMessageTextContains(string stringToMatch)
        {
            And(x => x.Message.ToUpper().Trim().Contains(stringToMatch.ToUpper().Trim()));

            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="stringToMatch"></param>
        /// <returns></returns>
        public DatabaseLogQuery WhereFileNameContains(string stringToMatch)
        {
            And(x => x.FileName.ToUpper().Trim().Contains(stringToMatch.ToUpper().Trim()));

            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="stringToMatch"></param>
        /// <returns></returns>
        public DatabaseLogQuery WhereMethodNameContains(string stringToMatch)
        {
            And(x => x.MethodName.ToUpper().Trim().Contains(stringToMatch.ToUpper().Trim()));

            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="stringToMatch"></param>
        /// <returns></returns>
        public DatabaseLogQuery WhereLineNumberIs(string stringToMatch)
        {
            And(x => x.LineNo.ToUpper().Trim().Contains(stringToMatch.ToUpper().Trim()));

            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="stringToMatch"></param>
        /// <returns></returns>
        public DatabaseLogQuery WhereErrorLevelIs(string stringToMatch)
        {
            And(x => x.ErrorLevel.ToUpper().Trim().Contains(stringToMatch.ToUpper().Trim()));

            return this;
        }

        public DatabaseLogQuery WhereMessageTextContains(string stringToMatch)
        {
            And(x => x.Message.ToUpper().Trim().Contains(stringToMatch.ToUpper().Trim()));

            return this;
        }
    }
}