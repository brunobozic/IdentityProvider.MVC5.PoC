using System.Web.Mvc;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Repository.EF.Queries.UserProfile;
using IdentityProvider.Services.ApplicationRoleService;
using IdentityProvider.Services.DbSeed;
using StructureMap;

namespace IdentityProvider.Controllers.Controllers
{
    public class HomeController : BaseController
    {
        [DefaultConstructor]
        public HomeController(
            ICookieStorageService cookieStorageService
            , IErrorLogService errorLogService
)
            : base(
                cookieStorageService
                , errorLogService
            )
        {
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Seed()
        {
            var dBSeeder = (DoSeed) DependencyResolver.Current.GetService(typeof(IDoSeed));
            var seedSuccessfull = false;

            seedSuccessfull = dBSeeder.Seed();

            ViewBag.Message = "Your seeding process went well.";

            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ProtectedActionForTesting()
        {
            ViewBag.Message = "Protected Action For Testing.";

            return View();
        }

        public ActionResult TestRepostoryQueries()
        {
            ViewBag.Message = "Protected Action For Testing.";

            var roleService =
                (ApplicationRoleService) DependencyResolver.Current.GetService(typeof(IApplicationRoleService));

            roleService.Query();

            return null;
            // return View(results);
        }
    }
}