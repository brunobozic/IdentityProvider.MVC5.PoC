using System.Web.Mvc;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Models.ViewModels.Error;
using StructureMap;

namespace IdentityProvider.Controllers.Controllers
{
    public class ErrorController : BaseController
    {
        private ICookieStorageService _cookieStorageService;

        [DefaultConstructor]
        public ErrorController(
            ICookieStorageService cookieStorageService
            , IErrorLogService errorLogService
        )
            : base(
                cookieStorageService
                , errorLogService
            )
        {
            _cookieStorageService = cookieStorageService;
        }

        public ViewResult Error()
        {
            var evm = (ErrorViewModel) TempData["ErrorViewModel"];

            return View(evm);
        }

        public ViewResult UserFriendlyError()
        {
            var evm = (HandleErrorInfo) TempData["HandleErrorInfo"];

            return View(evm);
        }
    }
}