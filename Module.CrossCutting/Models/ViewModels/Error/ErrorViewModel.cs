using System;
using System.Web.Mvc;

namespace IdentityProvider.Models.ViewModels.Error
{
    public class ErrorViewModel
    {
        public ErrorViewModel(Exception exception, string controllerName, string actionName)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            Exception = exception;
        }

        public ErrorViewModel(HandleErrorInfo handleErrorInfo)
        {
            ActionName = handleErrorInfo.ActionName;
            ControllerName = handleErrorInfo.ControllerName;
            Exception = handleErrorInfo.Exception;
        }

        public string ActionName { get; }
        public string ControllerName { get; }
        public Exception Exception { get; }
    }
}