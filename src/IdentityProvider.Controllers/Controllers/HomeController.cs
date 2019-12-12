using System.Web.Mvc;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
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
            , IApplicationConfiguration applicationConfiguration)
            : base(
            cookieStorageService
                  , errorLogService
                  , applicationConfiguration
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
            var dBSeeder = (DoSeed)DependencyResolver.Current.GetService(typeof(IDoSeed));
            bool seedSuccessfull = false;

            try
            {
                seedSuccessfull = dBSeeder.Seed();
            }
            catch (System.Exception)
            {
                throw;
            }

            ViewBag.Message = "Your seeding process went well.";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            Response.Redirect("");

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

            var rolsService =
                (ApplicationRoleService)DependencyResolver.Current.GetService(typeof(IApplicationRoleService));

            var results = rolsService.FetchReasourseAndOperationsGraph();


            return View(results);
        }
    }
}