using System.Web.Mvc;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Models.ViewModels.Error;

namespace IdentityProvider.Controllers.Controllers
{
    public class ErrorController : BaseController
    {
        private ICookieStorageService _cookieStorageService;

        public ErrorController(ICookieStorageService cookieStorageService) : base(cookieStorageService, null)
        {
            _cookieStorageService = cookieStorageService;
        }

        public ErrorController(ICookieStorageService cookieStorageService, IErrorLogService errorLogService) : base(
            cookieStorageService, errorLogService)
        {
        }

        public ViewResult Error()
        {
            var evm = (ErrorViewModel)TempData["ErrorViewModel"];

            return View(evm);
        }

        public ViewResult UserFriendlyError()
        {
            var evm = (HandleErrorInfo)TempData["HandleErrorInfo"];

            return View(evm);
        }
    }
}