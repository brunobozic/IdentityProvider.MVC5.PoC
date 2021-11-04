using System.Web.Mvc;

namespace IdentityProvider.Controllers.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            Response.Redirect("Operation/OperationsGetAllPaged", true);

            return View();
        }
    }
}