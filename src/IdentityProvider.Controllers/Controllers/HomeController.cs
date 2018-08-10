using System.Web.Mvc;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Services;
using IdentityProvider.Services.ApplicationRoleService;
using IdentityProvider.Services.DbSeed;

namespace IdentityProvider.Controllers.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ICookieStorageService cookieStorageService, IErrorLogService errorLogService) : base(
            cookieStorageService, errorLogService)
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

        public ActionResult Seed()
        {
            var dBSeeder = (DoSeed)DependencyResolver.Current.GetService(typeof(IDoSeed));
            var seedSuccessfull = dBSeeder.Seed();

            ViewBag.Message = "Your seeding process went well.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";


            return View();
        }

        public ActionResult ProtectedActionForTesting()
        {
            ViewBag.Message = "Protected Action For Testing.";

            return View();
        }

        public ActionResult TestRepostoryQueries()
        {
            ViewBag.Message = "Protected Action For Testing.";

            var rolsService =
                (ApplicationRoleService) DependencyResolver.Current.GetService(typeof(IApplicationRoleService));

            var results = rolsService.FetchReasourseAndOperationsGraph();


            return View(results);
        }
    }
}