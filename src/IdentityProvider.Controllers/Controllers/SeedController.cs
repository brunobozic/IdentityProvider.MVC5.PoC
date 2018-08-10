using System.Web.Mvc;
using System.Web.Routing;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Services.DbSeed;
using StructureMap;

namespace IdentityProvider.Controllers.Controllers
{
    public class SeedController : BaseController
    {
        private readonly IDoSeed _doSeed;
        private readonly ICookieStorageService _cookieStorageService;
        private readonly IErrorLogService _errorLogService;
        [DefaultConstructor]
        public SeedController(
            IDoSeed doSeed
            , ICookieStorageService cookieStorageService
            , IErrorLogService errorLogService
            , ICookieStorageService cookieStorageService1
            )
            : base(
                  cookieStorageService
                  , errorLogService
                  )
        {
            _doSeed = doSeed;
            _cookieStorageService = cookieStorageService1;
            _errorLogService = errorLogService;
        }

        public ActionResult Index()
        {
            var seedOk = _doSeed.Seed();

            return View();
        }

        public void Execute(RequestContext requestContext)
        {

        }
    }
}