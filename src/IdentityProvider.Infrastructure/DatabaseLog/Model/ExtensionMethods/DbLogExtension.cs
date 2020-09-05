using IdentityProvider.Infrastructure.DatabaseLog.DTOs;
using IdentityProvider.Infrastructure.Logging.Log4Net;
using System;

namespace IdentityProvider.Infrastructure.DatabaseLog.Model.ExtensionMethods
{
    public static class LoggingEventDtoExtension
    {
        /// <summary>
        /// </summary>
        /// <param name="loggingEventDto"></param>
        /// <returns></returns>
        public static DbLog ConvertToDbLog(this LoggingEventDto loggingEventDto)
        {
            const string replacementToken = "N/A";
            var myLog = new DbLog();

            try
            {
                #region Map LoggingEvent (Log4Net)

                myLog.ADUser = loggingEventDto.Domain ?? replacementToken;
                myLog.CreatedDate = loggingEventDto.TimeStamp;
                myLog.UserId = 1;
                myLog.UserName = loggingEventDto.UserName ?? replacementToken;
                myLog.TrackingNo = replacementToken;
                myLog.Operation = replacementToken;
                myLog.InputParams = replacementToken;
                myLog.OutputParams = replacementToken;
                myLog.FileName = replacementToken;
                myLog.ErrorLevel = loggingEventDto.DisplayName ?? replacementToken;
                myLog.ModifiedDate = loggingEventDto.TimeStamp;
                myLog.AbsoluteUrl = replacementToken;
                myLog.ClientBrowser = replacementToken;
                myLog.RemoteHost = replacementToken;
                myLog.Path = replacementToken;
                myLog.Query = replacementToken;
                myLog.RequestId = replacementToken;
                myLog.SessionId = replacementToken;
                myLog.ExceptionType = replacementToken;
                myLog.ExceptionMessage = replacementToken;
                myLog.ExceptionStackTrace = replacementToken;
                myLog.Message = loggingEventDto.RenderedMessage ?? replacementToken;
                myLog.AssemblyQualifiedName = replacementToken;
                myLog.Namespace = replacementToken;
                myLog.LogSource = replacementToken;

                #endregion

                #region Map Properties

                if (loggingEventDto.Properties["RefererUrl"] != null)
                {
                    var l = loggingEventDto.Properties["RefererUrl"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.AbsoluteUrl = l;
                }

                if (loggingEventDto.Properties["UserAgent"] != null)
                {
                    var l = loggingEventDto.Properties["UserAgent"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.ClientBrowser = l;
                }

                if (loggingEventDto.Properties["RemoteHost"] != null)
                {
                    var l = loggingEventDto.Properties["RemoteHost"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.RemoteHost = l;
                }

                if (loggingEventDto.Properties["Path"] != null)
                {
                    var l = loggingEventDto.Properties["Path"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.Path = l;
                }

                if (loggingEventDto.Properties["Query"] != null)
                {
                    var l = loggingEventDto.Properties["Query"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.Query = l;
                }

                if (loggingEventDto.Properties["RequestId"] != null)
                {
                    var l = loggingEventDto.Properties["RequestId"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.RequestId = l;
                }

                if (loggingEventDto.Properties["SessionId"] != null)
                {
                    var l = loggingEventDto.Properties["SessionId"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.SessionId = l;
                }

                if (loggingEventDto.Properties["ExceptionType"] != null)
                {
                    var l = loggingEventDto.Properties["ExceptionType"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.ExceptionType = l;
                }

                if (loggingEventDto.Properties["ExceptionMessage"] != null)
                {
                    var l = loggingEventDto.Properties["ExceptionMessage"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.ExceptionMessage = l;
                }

                if (loggingEventDto.Properties["ExceptionStackTrace"] != null)
                {
                    var l = loggingEventDto.Properties["ExceptionStackTrace"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.ExceptionStackTrace = l;
                }

                if (loggingEventDto.Properties["AssemblyQualifiedName"] != null)
                {
                    var l = loggingEventDto.Properties["AssemblyQualifiedName"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.AssemblyQualifiedName = l;
                }

                if (loggingEventDto.Properties["Namespace"] != null)
                {
                    var l = loggingEventDto.Properties["Namespace"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.Namespace = l;
                }

                if (loggingEventDto.Properties["LogSource"] != null)
                {
                    var l = loggingEventDto.Properties["LogSource"].ToString();
                    if (!string.IsNullOrEmpty(l))
                        myLog.LogSource = l;
                }


                // myLog.InnerExceptionMessage = loggingEventDto.Properties["InnerException.Message"].ToString();
                // myLog.InnerExceptionSource = loggingEventDto.Properties["InnerException.Source"].ToString();
                // myLog.InnerExceptionStackTrace = loggingEventDto.Properties["InnerException.StackTrace"].ToString();
                // myLog.InnerExceptionTargetSite = loggingEventDto.Properties["InnerException.TargetSite"].ToString();

                try
                {
                    if (loggingEventDto.LocationInformation != null)
                    {
                        if (loggingEventDto.LocationInformation.ClassName != null)
                            myLog.Method += loggingEventDto.LocationInformation.ClassName;

                        if (loggingEventDto.LocationInformation.FileName != null)
                            myLog.FileName += loggingEventDto.LocationInformation.FileName;

                        if (loggingEventDto.LocationInformation.FullInfo != null)
                            myLog.MethodName += loggingEventDto.LocationInformation.MethodName;

                        if (loggingEventDto.LocationInformation.LineNumber != null)
                            myLog.LineNo += loggingEventDto.LocationInformation.LineNumber;

                        if (loggingEventDto.LocationInformation.MethodName != null)
                            myLog.MethodName += loggingEventDto.LocationInformation.MethodName;

                        if (loggingEventDto.LocationInformation.StackFrames != null)
                        {
                            // TODO: handle the entire tree of stack frames (may prove to be costly DB-wise)
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                #endregion
            }
            catch (Exception ex)
            {
                myLog.ExceptionMessage += $"		====>		Exception saving exception: [ {ex.Message} ]";

                // Log to file...
                Log4NetLoggingFactory.GetLogger().LogWarning(null, myLog.ExceptionMessage, ex);
            }


            return myLog;
        }
    }
}