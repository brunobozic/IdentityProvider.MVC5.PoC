using IdentityProvider.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackableEntities;

namespace IdentityProvider.Infrastructure.DatabaseLog.Model
{
    [Table("DatabaseLog", Schema = "Log")]
    public class DbLog : DomainEntity<int>, IDbLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Operation { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string TrackingNo { get; set; }
        public string ErrorLevel { get; set; }
        public string InputParams { get; set; }
        public string OutputParams { get; set; }
        public string FileName { get; set; }
        public string MethodName { get; set; }
        public string LineNo { get; set; }
        public string ColumnNo { get; set; }
        public string AbsoluteUrl { get; set; }
        public string ADUser { get; set; }
        public string ClientBrowser { get; set; }
        public string RemoteHost { get; set; }
        public string Path { get; set; }
        public string Query { get; set; }
        public string Referrer { get; set; }
        public string RequestId { get; set; }
        public string SessionId { get; set; }
        public string Method { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string InnerExceptionMessage { get; set; }
        public string InnerExceptionSource { get; set; }
        public string InnerExceptionStackTrace { get; set; }
        public string InnerExceptionTargetSite { get; set; }
        public string AssemblyQualifiedName { get; set; }
        public string Namespace { get; set; }
        public string LogSource { get; set; }
        public DateTime TimeStamp { get; set; }

        public TrackingState TrackingState { get; set; }
        public ICollection<string> ModifiedProperties { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}