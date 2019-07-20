using System.Web.Mvc;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
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

        [DefaultConstructor]
        public SeedController(
            IDoSeed doSeed
            , ICookieStorageService cookieStorageService
            , IErrorLogService errorLogService
            , ICookieStorageService cookieStorageService1
            , IApplicationConfiguration applicationConfiguration
            )
            : base(
                  cookieStorageService
                  , errorLogService
                  , applicationConfiguration
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
    }
}