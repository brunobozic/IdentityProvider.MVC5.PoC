
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Models.ViewModels.Account;
using IdentityProvider.Services;
using IdentityProvider.Services.UserProfileService;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Owin.Security;
using StructureMap;
using System.Linq;
using System.Threading.Tasks;


namespace IdentityProvider.Controllers.Controllers
{
    public class AccountAdministrationController : BaseController
    {
        //    private readonly IUserProfileAdministrationService _administrationService;
        //    private readonly ApplicationSignInManager _signInManager;
        //    private readonly IWebSecurity _webSecurity;
        //    private IAuthenticationManager _authenticationManager;
        //    private ApplicationUserManager _userManager;

        //    [DefaultConstructor]
        //    public AccountAdministrationController(
        //        IWebSecurity webSecurity
        //        , IUserProfileAdministrationService administrationService
        //        , ICookieStorageService cookieStorageService
        //        , IErrorLogService errorLogService
        //        , IApplicationConfiguration applicationConfiguration
        //        , ApplicationSignInManager signInManager
        //        , ApplicationUserManager userManager
        //        , IAuthenticationManager authenticationManager
        //    )
        //        : base(
        //            cookieStorageService
        //            , errorLogService
        //            , applicationConfiguration
        //        )
        //    {
        //        _administrationService = administrationService;
        //        _webSecurity = webSecurity;
        //        _errorLogService = errorLogService;
        //        _userManager = userManager;
        //        _signInManager = signInManager;
        //        _authenticationManager = authenticationManager;
        //    }

        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing && _userManager != null)
        //        {
        //            _userManager.Dispose();
        //            _userManager = null;
        //        }

        //        base.Dispose(disposing);
        //    }

        //    //
        //    // GET: /Manage/AddPhoneNumber
        //    public ActionResult AddPhoneNumber()
        //    {
        //        return View();
        //    }

        //    //
        //    // POST: /Manage/AddPhoneNumber
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        //    {
        //        if (!ModelState.IsValid)
        //            return View(model);
        //        // Generate the token and send it
        //        var code = await _userManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
        //        if (_userManager.SmsService != null)
        //        {
        //            var message = new IdentityMessage
        //            {
        //                Destination = model.Number,
        //                Body = "Your security code is: " + code
        //            };
        //            await _userManager.SmsService.SendAsync(message);
        //        }

        //        return RedirectToAction("VerifyPhoneNumber", new {PhoneNumber = model.Number});
        //    }

        //    //
        //    // GET: /Manage/ChangePassword
        //    public ActionResult ChangePassword()
        //    {
        //        return View();
        //    }

        //    //
        //    // POST: /Manage/ChangePassword
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        //    {
        //        if (!ModelState.IsValid)
        //            return View(model);
        //        var result =
        //            await _userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
        //        if (result.Succeeded)
        //        {
        //            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
        //            if (user != null)
        //                await _signInManager.SignInAsync(user, false, false);
        //            return RedirectToAction("Index", new {Message = ManageMessageId.ChangePasswordSuccess});
        //        }

        //        AddErrors(result);
        //        return View(model);
        //    }

        //    //
        //    // POST: /Manage/DisableTwoFactorAuthentication
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> DisableTwoFactorAuthentication()
        //    {
        //        await _userManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
        //        var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
        //        if (user != null)
        //            await _signInManager.SignInAsync(user, false, false);
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    //
        //    // POST: /Manage/EnableTwoFactorAuthentication
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> EnableTwoFactorAuthentication()
        //    {
        //        await _userManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
        //        var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
        //        if (user != null)
        //            await _signInManager.SignInAsync(user, false, false);
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    //
        //    // GET: /AccountAdministration/Index
        //    public async Task<ActionResult> Index()
        //    {
        //        var userId = User.Identity.GetUserId();

        //        if (userId != null)
        //        {
        //            var retVal = new AccountAdministrationLandingViewModel
        //            {
        //                HasPassword = HasPassword(),
        //                PhoneNumber = await _userManager.GetPhoneNumberAsync(userId),
        //                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(userId),
        //                Logins = await _userManager.GetLoginsAsync(userId),
        //                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
        //            };

        //            return PartialView("Partial/_accountAdministrationLanding", retVal);
        //        }

        //        return PartialView("Partial/_accountAdministrationLanding",
        //            new AccountAdministrationLandingViewModel
        //            {
        //                HasPassword = false, BrowserRemembered = false, Logins = null, PhoneNumber = string.Empty,
        //                TwoFactor = false
        //            });
        //    }

        //    //
        //    // POST: /Manage/LinkLogin
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult LinkLogin(string provider)
        //    {
        //        // Request a redirect to the external login provider to link a login for the current user
        //        return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        //    }

        //    //
        //    // GET: /Manage/LinkLoginCallback
        //    public async Task<ActionResult> LinkLoginCallback()
        //    {
        //        var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
        //        if (loginInfo == null)
        //            return RedirectToAction("ManageLogins", new {Message = ManageMessageId.Error});
        //        var result = await _userManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
        //        return result.Succeeded
        //            ? RedirectToAction("ManageLogins")
        //            : RedirectToAction("ManageLogins", new {Message = ManageMessageId.Error});
        //    }

        //    //
        //    // GET: /Manage/ManageLogins
        //    public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        //    {
        //        ViewBag.StatusMessage =
        //            message == ManageMessageId.RemoveLoginSuccess
        //                ? "The external login was removed."
        //                : message == ManageMessageId.Error
        //                    ? "An error has occurred."
        //                    : string.Empty;
        //        var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
        //        if (user == null)
        //            return View("Error");
        //        var userLogins = await _userManager.GetLoginsAsync(User.Identity.GetUserId());
        //        var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes()
        //            .Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
        //        ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
        //        return View(new ManageLoginsViewModel
        //        {
        //            CurrentLogins = userLogins,
        //            OtherLogins = otherLogins
        //        });
        //    }

        //    //
        //    // POST: /Manage/RemoveLogin
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        //    {
        //        ManageMessageId? message;
        //        var result = await _userManager.RemoveLoginAsync(User.Identity.GetUserId(),
        //            new UserLoginInfo(loginProvider, providerKey));
        //        if (result.Succeeded)
        //        {
        //            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
        //            if (user != null)
        //                await _signInManager.SignInAsync(user, false, false);
        //            message = ManageMessageId.RemoveLoginSuccess;
        //        }
        //        else
        //        {
        //            message = ManageMessageId.Error;
        //        }

        //        return RedirectToAction("ManageLogins", new {Message = message});
        //    }

        //    //
        //    // POST: /Manage/RemovePhoneNumber
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> RemovePhoneNumber()
        //    {
        //        var result = await _userManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
        //        if (!result.Succeeded)
        //            return RedirectToAction("Index", new {Message = ManageMessageId.Error});
        //        var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
        //        if (user != null)
        //            await _signInManager.SignInAsync(user, false, false);
        //        return RedirectToAction("Index", new {Message = ManageMessageId.RemovePhoneSuccess});
        //    }

        //    //
        //    // GET: /Manage/SetPassword
        //    public ActionResult SetPassword()
        //    {
        //        return View();
        //    }

        //    //
        //    // POST: /Manage/SetPassword
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var result = await _userManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
        //            if (result.Succeeded)
        //            {
        //                var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
        //                if (user != null)
        //                    await _signInManager.SignInAsync(user, false, false);
        //                return RedirectToAction("Index", new {Message = ManageMessageId.SetPasswordSuccess});
        //            }

        //            AddErrors(result);
        //        }

        //        // If we got this far, something failed, redisplay form
        //        return View(model);
        //    }

        //    public ViewResult UserDetails(int id)
        //    {
        //        //var response = new UserDetailsGetByIdResponse();
        //        //var request = new UserDetailsGetByIdRequest {UserProfileId = id};

        //        //// SetViewBagData(id);
        //        //return View(response.UserProfile);

        //        return null;
        //    }

        //    public ActionResult UsersActiveGetAll()
        //    {
        //        return View(_webSecurity.UsersActiveGetAll());
        //    }

        //    //
        //    // GET: /Manage/VerifyPhoneNumber
        //    public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        //    {
        //        var code = await _userManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
        //        // Send an SMS through the SMS provider to verify the phone number
        //        return phoneNumber == null
        //            ? View("Error")
        //            : View(new VerifyPhoneNumberViewModel {PhoneNumber = phoneNumber});
        //    }

        //    //
        //    // POST: /Manage/VerifyPhoneNumber
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        //    {
        //        if (!ModelState.IsValid)
        //            return View(model);
        //        var result =
        //            await _userManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
        //        if (result.Succeeded)
        //        {
        //            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
        //            if (user != null)
        //                await _signInManager.SignInAsync(user, false, false);
        //            return RedirectToAction("Index", new {Message = ManageMessageId.AddPhoneSuccess});
        //        }

        //        // If we got this far, something failed, redisplay form
        //        ModelState.AddModelError(string.Empty, "Failed to verify phone");
        //        return View(model);
        //    }

        //    #region Helpers

        //    // Used for XSRF protection when adding external logins
        //    private const string XsrfKey = "XsrfId";

        //    private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        //    private void AddErrors(IdentityResult result)
        //    {
        //        foreach (var error in result.Errors)
        //            ModelState.AddModelError(string.Empty, error);
        //    }

        //    private bool HasPassword()
        //    {
        //        var user = _userManager.FindById(User.Identity.GetUserId());
        //        return user?.PasswordHash != null;
        //    }

        //    private bool HasPhoneNumber()
        //    {
        //        var user = _userManager.FindById(User.Identity.GetUserId());
        //        return user?.PhoneNumber != null;
        //    }

        //    public enum ManageMessageId
        //    {
        //        AddPhoneSuccess,
        //        ChangePasswordSuccess,
        //        SetTwoFactorSuccess,
        //        SetPasswordSuccess,
        //        RemoveLoginSuccess,
        //        RemovePhoneSuccess,
        //        Error
        //    }

        //    #endregion
        public AccountAdministrationController(ICookieStorageService cookieStorageService, IErrorLogService errorLogService) : base(cookieStorageService, errorLogService)
        {
        }
    }
}