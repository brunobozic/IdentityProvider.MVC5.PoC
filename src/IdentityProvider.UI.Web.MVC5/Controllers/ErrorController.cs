using System.Web.Mvc;

namespace IdentityProvider.UI.Web.MVC5.Controllers
{
    public class ErrorController : Controller
    {
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