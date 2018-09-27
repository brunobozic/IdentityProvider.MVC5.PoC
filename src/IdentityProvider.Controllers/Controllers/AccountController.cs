using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IdentityProvider.Controllers.Helpers;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.Cookies;
using IdentityProvider.Infrastructure.Logging.Serilog.Providers;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Models.ViewModels.Account;
using IdentityProvider.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using StructureMap;

namespace IdentityProvider.Controllers.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly ICookieStorageService _cookieStorageService;
        private readonly ApplicationUserManager _userManager;
        private readonly IWebSecurity _webSecurity;
        private IAuthenticationManager _authenticationManager;
        private ApplicationSignInManager _signInManager;
        [DefaultConstructor]
        public AccountController(
            IWebSecurity webSecurity
            , ICookieStorageService cookieStorageService
            , ApplicationUserManager userManager
            , ApplicationSignInManager signInManager
            , IAuthenticationManager authenticationManager
            , IErrorLogService errorLogService 
            , IApplicationConfiguration applicationConfiguration 
            )
            : base(
                cookieStorageService
                , errorLogService
                , applicationConfiguration
            )
        {
            _webSecurity = webSecurity ?? throw new ArgumentNullException(nameof(webSecurity));
            _cookieStorageService =
                cookieStorageService ?? throw new ArgumentNullException(nameof(cookieStorageService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _authenticationManager =
                authenticationManager ?? throw new ArgumentNullException(nameof(authenticationManager));
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login( string returnUrl )
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginBootstrap( LoginViewModel model , string returnUrl )
        {
            if (!ModelState.IsValid)
                return View(model);

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _webSecurity.PasswordSignInAsync(model.Username , model.Password , model.RememberMe , true);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode" , new { ReturnUrl = returnUrl , model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("" , "Invalid login attempt.");
                    return View(model);
            }
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login( LoginViewModel model , string returnUrl )
        {
            if (!ModelState.IsValid)
                return View(model);

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _webSecurity.PasswordSignInAsync(model.Username , model.Password , model.RememberMe , true);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode" , new { ReturnUrl = returnUrl , model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("" , "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode( string provider , string returnUrl , bool rememberMe )
        {
            // Require that the user has already logged in via username/password or external login
            if (!await _webSecurity.HasBeenVerifiedAsync())
                return View("Error");

            return View(new VerifyCodeViewModel { Provider = provider , ReturnUrl = returnUrl , RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode( VerifyCodeViewModel model )
        {
            if (!ModelState.IsValid)
                return View(model);

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await _webSecurity.TwoFactorSignInAsync(model.Provider , model.Code , model.RememberMe ,
                model.RememberBrowser);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("" , "Invalid code.");
                    return View(model);
            }
        }

        #region Register a new account

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register( RegisterViewModel model )
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.DesiredUserName ,
                    Email = model.Email ,
                    FirstName = model.FirstName ,
                    LastName = model.LastName,
                  
                };

                var result = await _webSecurity.CreateAsync(user , model.Password);

                if (result.Succeeded)
                {
                    // await _webSecurity.SignInAsync(user , false , false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);

                    var callbackUrl = Url.Action("ConfirmEmail" , "Account" , new { userId = user.Id , code } ,
                        Request.Url.Scheme);

                    await _userManager.SendEmailAsync(user.Id , "Confirm your account (md.proof of concept application)" ,
                        "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index" , "Home");
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion Register a new account


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail( string userId , string code )
        {
            if (userId == null || code == null)
                return View("Error");

            var result = await _webSecurity.ConfirmEmailAsync(userId , code);

            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword( ForgotPasswordViewModel model )
        {
            if (ModelState.IsValid)
            {
                var user = await _webSecurity.FindByNameAsync(model.Email);

                if (user == null || !await _webSecurity.IsEmailConfirmedAsync(user.Id))
                    return View("ForgotPasswordConfirmation");

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);

                var callbackUrl = Url.Action("ResetPassword" , "Account" , new { userId = user.Id , code } ,
                    Request.Url.Scheme);

                await _userManager.SendEmailAsync(user.Id , "Reset Password" ,
                    "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

                return RedirectToAction("ForgotPasswordConfirmation" , "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword( string code )
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword( ResetPasswordViewModel model )
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _webSecurity.FindByNameAsync(model.Email);

            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation" , "Account");

            var result = await _webSecurity.ResetPasswordAsync(user.Id , model.Code , model.Password);

            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation" , "Account");

            AddErrors(result);

            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin( string provider , string returnUrl )
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider ,
                Url.Action("ExternalLoginCallback" , "Account" , new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode( string returnUrl , bool rememberMe )
        {
            var userId = await _webSecurity.GetVerifiedUserIdAsync();

            if (userId == null)
                return View("Error");

            var userFactors = await _webSecurity.GetValidTwoFactorProvidersAsync(userId)
                ;
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose , Value = purpose })
                .ToList();

            return View(new SendCodeViewModel
            {
                Providers = factorOptions ,
                ReturnUrl = returnUrl ,
                RememberMe = rememberMe
            });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode( SendCodeViewModel model )
        {
            if (!ModelState.IsValid)
                return View();

            // Generate the token and send it
            if (!await _webSecurity.SendTwoFactorCodeAsync(model.SelectedProvider))
                return View("Error");

            return RedirectToAction("VerifyCode" ,
                new { Provider = model.SelectedProvider , model.ReturnUrl , model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback( string returnUrl )
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
                return RedirectToAction("Login");

            // Sign in the user with this external login provider if the user already has a login
            var result = await _webSecurity.ExternalSignInAsync(loginInfo , false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode" , new { ReturnUrl = returnUrl , RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation" ,
                        new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation( ExternalLoginConfirmationViewModel model ,
            string returnUrl )
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index" , "Manage");

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();

                if (info == null)
                    return View("ExternalLoginFailure");

                var user = new ApplicationUser { UserName = model.Email , Email = model.Email };

                var result = await _webSecurity.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await _webSecurity.AddLoginAsync(user.Id , info.Login);

                    if (result.Succeeded)
                    {
                        await _webSecurity.SignInAsync(user , false , false);

                        return RedirectToLocal(returnUrl);
                    }
                }

                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index" , "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose( bool disposing )
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors( IdentityResult result )
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("" , error);
        }

        private ActionResult RedirectToLocal( string returnUrl )
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index" , "Home");
        }

        #endregion
    }
}