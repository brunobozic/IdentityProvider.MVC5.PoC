using System.Web.Mvc;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Services;
using IdentityProvider.Services.UserProfileService;

namespace IdentityProvider.Controllers.Controllers
{
    public class AdministrationController : BaseController
    {
        private readonly IUserProfileAdministrationService _administrationService;
        private readonly IWebSecurity _webSecurity;

        public AdministrationController(IWebSecurity webSecurity,
            IUserProfileAdministrationService administrationService
            , ICookieStorageService cookieStorageService
            , IErrorLogService errorLogService
        )
            : base(cookieStorageService, errorLogService)
        {
            _administrationService = administrationService;
            _webSecurity = webSecurity;
            _errorLogService = errorLogService;
        }

        public ActionResult Index()
        {
            return View(_webSecurity.UsersActiveGetAll());
        }

        public ViewResult UserDetails(int id)
        {
            //var response = new UserDetailsGetByIdResponse();
            //var request = new UserDetailsGetByIdRequest {UserProfileId = id};

            //// SetViewBagData(id);
            //return View(response.UserProfile);

            return null;
        }
    }
}