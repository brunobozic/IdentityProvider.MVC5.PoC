using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using IdentityProvider.Infrastructure;
using IdentityProvider.Infrastructure.ApplicationConfiguration;
using IdentityProvider.Infrastructure.ApplicationContext;
using IdentityProvider.Infrastructure.Logging.Log4Net;
using IdentityProvider.Models.Domain.Account;
using IdentityProvider.Repository.EF.Factories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Module.Repository.EF;
using Module.Repository.EF.RowLevelSecurity;
using Module.Repository.EF.UnitOfWorkInterfaces;
using TrackableEntities;

namespace IdentityProvider.Services
{
    public class WebSecurity : IWebSecurity, IDisposable
    {
        private readonly IApplicationConfiguration _configurationRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private ILog4NetLoggingService _loggingService;
        private ApplicationUserManager _userManager;
        private readonly ICachedUserAuthorizationGrantsProvider _cachedUserAuthorizationGrantsProvider;

        //public IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;

        //public bool IsAuthenticated => Thread.CurrentPrincipal.Identity.IsAuthenticated;

        //public string CurrentUserName => Thread.CurrentPrincipal.Identity.GetUserName().Trim();

        //public string GetCurrentUserId => Thread.CurrentPrincipal.Identity.GetUserId().Trim();

        //public IIdentityMessageService SmsService => _userManager.SmsService;

        //public UserManager<ApplicationUser> ThisUserManager => _userManager;
        #region Ctor

        public WebSecurity(
            IUnitOfWorkAsync unitOfWorkAsync
            , ILog4NetLoggingService loggingService
            , IMapper mapper
            , ApplicationSignInManager signInManager
            , ApplicationUserManager userManager
            , ICachedUserAuthorizationGrantsProvider cachedUserAuthorizationGrantsProvider
        )
        {
            _userManager = userManager;
            _cachedUserAuthorizationGrantsProvider = cachedUserAuthorizationGrantsProvider;
            _signInManager = signInManager;
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWorkAsync = unitOfWorkAsync ?? throw new ArgumentNullException(nameof(unitOfWorkAsync));
            // _userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }


        public WebSecurity(
            IContextProvider contextService
            , IApplicationConfiguration configurationRepository
            , ApplicationUserManager userManager
            , ApplicationSignInManager signInManager
        )
        {
            _configurationRepository = configurationRepository;
            _userManager = userManager;
            _signInManager = signInManager;

            try
            {
                var dbContextAsync = DataContextFactory.GetDataContextAsync();
                dbContextAsync.GetDatabase().Initialize(true);

                if (_loggingService == null)
                    _loggingService = Log4NetLoggingFactory.GetLogger();

                _unitOfWorkAsync = new UnitOfWork(dbContextAsync, new RowAuthPoliciesContainer(_cachedUserAuthorizationGrantsProvider));
                // UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContextAsync as DbContext));
            }
            catch (Exception ex)
            {
                FetchLoggerAndLog(ex);
            }
        }

        #endregion Ctor

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            if (_unitOfWorkAsync != null)
                try
                {
                }
                catch (Exception disposeException)
                {
                    FetchLoggerAndLog(disposeException);

                    _loggingService.LogWarning(this, "IDisposable problem disposing", disposeException);
                }
        }

        #endregion Dispose


        public List<ApplicationUser> UsersActiveGetAll()
        {
            return
                _unitOfWorkAsync.Repository<ApplicationUser>()
                    .Queryable()
                    .Where(u => u.Active)
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .ToList();
        }

        private ApplicationUser GetUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));

            return _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Find(userName.Trim());
        }

        public async Task<ApplicationUser> GetUserAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));

            ApplicationUser retVal = null;

            try
            {
                retVal =
                    await _unitOfWorkAsync.RepositoryAsync<ApplicationUser>()
                        .FindAsync(userName.Trim());
            }
            catch (Exception ex)
            {
                FetchLoggerAndLog(ex);
            }

            return retVal;
        }

        public string GetUserIdFromPasswordToken(string passwordResetToken)
        {
            if (string.IsNullOrEmpty(passwordResetToken))
                throw new ArgumentNullException(nameof(passwordResetToken));

            string id = null;

            var user = _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Queryable()
                .SingleOrDefault(u => u.PasswordResetToken.Equals(passwordResetToken.Trim()));

            if (user != null)
                id = user.Id;

            return id?.Trim();
        }

        private void FetchLoggerAndLog(Exception ex)
        {
            if (_loggingService != null) return;
            _loggingService = Log4NetLoggingFactory.GetLogger();
            _loggingService.LogWarning(this, "IDisposable problem disposing", ex);
        }


        #region Identity 2.0

        public async Task<ApplicationUser> GetUserByUserNameAsync(string username)
        {
            return await _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Queryable()
                .Where(i => i.UserName.ToUpper().Equals(username.ToUpper().Trim())).SingleOrDefaultAsync();
        }

        public int? UpdateUser(ApplicationUser user)
        {
            user.TrackingState = TrackingState.Modified;
            var update = _userManager.Update(user);

            if (update.Succeeded)
                return 1;
            return null;
        }

        public async Task<SignInStatus> PasswordSignInAsync(string modelEmail, string modelPassword,
            bool modelRememberMe, bool shouldLockout)
        {
            return await _signInManager.PasswordSignInAsync(modelEmail, modelPassword, modelRememberMe, shouldLockout);
        }

        public async Task<bool> HasBeenVerifiedAsync()
        {
            return await _signInManager.HasBeenVerifiedAsync();
        }

        public async Task<SignInStatus> TwoFactorSignInAsync(string modelProvider, string modelCode, bool isPersistent,
            bool rememberBrowser)
        {
            return await _signInManager.TwoFactorSignInAsync(modelProvider, modelCode, isPersistent, rememberBrowser);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            user.TrackingState = TrackingState.Added;
            return await _userManager.CreateAsync(user, password);
        }

        public async Task SignInAsync(ApplicationUser user, bool isPersistent, bool rememberBrowser)
        {
            if (user.Active && !user.IsDeleted)
                await _signInManager.SignInAsync(user, isPersistent, rememberBrowser);
            else
                throw new AuthenticationException(
                    "Would not sign the user in, the user has been deleted on marked inactive.");
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            return await _userManager.ConfirmEmailAsync(userId, code);
        }

        public async Task<ApplicationUser> FindByNameAsync(string modelEmail)
        {
            return await _userManager.FindByNameAsync(modelEmail);
        }

        public async Task<bool> IsEmailConfirmedAsync(string id)
        {
            return await _userManager.IsEmailConfirmedAsync(id);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string id, string modelCode, string modelPassword)
        {
            return await _userManager.ResetPasswordAsync(id, modelCode, modelPassword);
        }

        public async Task<string> GetVerifiedUserIdAsync()
        {
            return await _signInManager.GetVerifiedUserIdAsync();
        }

        public async Task<IList<string>> GetValidTwoFactorProvidersAsync(string userId)
        {
            return await _userManager.GetValidTwoFactorProvidersAsync(userId);
        }

        public async Task<bool> SendTwoFactorCodeAsync(string modelSelectedProvider)
        {
            return await _signInManager.SendTwoFactorCodeAsync(modelSelectedProvider);
        }

        public async Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo, bool isPersistent)
        {
            return await _signInManager.ExternalSignInAsync(loginInfo, isPersistent);
        }

        public async Task<IdentityResult> AddLoginAsync(int id, UserLoginInfo login)
        {
            return await _userManager.AddLoginAsync(id.ToString(), login);
        }

        public async Task<IdentityResult> AddLoginAsync(string id, UserLoginInfo login)
        {
            return await _userManager.AddLoginAsync(id, login);
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            user.TrackingState = TrackingState.Added;
            return _userManager.CreateAsync(user);
        }

        #endregion Identity 2.0


        //public void RemovePasswordToken(string userName)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    var user = _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Queryable()
        //        .SingleOrDefault(u => u.Id.Equals(userName.Trim()));

        //    if (user != null)
        //        user.PasswordResetToken = null;

        //    _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Update(user);
        //    _unitOfWorkAsync.SaveChanges();
        //}

        //public bool Login(string userName, string password, bool persistCookie = false)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    if (string.IsNullOrEmpty(password))
        //        throw new ArgumentNullException(nameof(password));

        //    var user = _userManager.Find(userName.Trim(), password.Trim());

        //    if (user != null && user.IsConfirmed && user.Active && !user.IsDeleted)
        //    {
        //        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

        //        var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

        //        identity.AddClaim(new Claim(ClaimTypes.Email, user.Email.Trim()));
        //        AuthenticationManager.SignIn(new AuthenticationProperties {IsPersistent = persistCookie}, identity);
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //public bool ChangePassword(string userName, string oldPassword, string newPassword)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));
        //    if (string.IsNullOrEmpty(oldPassword))
        //        throw new ArgumentNullException(nameof(oldPassword));
        //    if (string.IsNullOrEmpty(newPassword))
        //        throw new ArgumentNullException(nameof(newPassword));

        //    var user = _userManager.FindByName(userName.Trim());

        //    if (user == null)
        //        return false;

        //    var result = _userManager.ChangePassword(user.Id.Trim(), oldPassword.Trim(), newPassword.Trim());

        //    return result.Succeeded;
        //}

        //public bool ConfirmAccount(string accountConfirmationToken)
        //{
        //    if (string.IsNullOrEmpty(accountConfirmationToken))
        //        throw new ArgumentNullException(nameof(accountConfirmationToken));

        //    var user =
        //        _unitOfWorkAsync.RepositoryAsync<ApplicationUser>()
        //            .Queryable()
        //            .FirstOrDefault(u => u.ConfirmationToken.Equals(accountConfirmationToken.Trim()));

        //    if (user == null) return false;

        //    user.IsConfirmed = true;
        //    user.TrackingState = TrackingState.Modified;

        //    _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Update(user);
        //    _unitOfWorkAsync.SaveChanges();

        //    return true;
        //}

        //public void Logout()
        //{
        //    AuthenticationManager.SignOut();
        //}

        //public bool IsConfirmed(string userName)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    var isConfirmed = false;

        //    var user = GetUser(userName);

        //    if (user != null)
        //        isConfirmed = user.IsConfirmed;

        //    return isConfirmed;
        //}

        //public bool IsActive(string userName)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    var isActive = false;

        //    var user = GetUser(userName);

        //    if (user != null)
        //        isActive = user.Active;

        //    return isActive;
        //}

        //public bool IsDeleted(string userName)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    var isDeleted = false;

        //    var user = GetUser(userName);

        //    if (user != null)
        //        isDeleted = user.IsDeleted;

        //    return isDeleted;
        //}

        //public bool ResetPassword(string passwordResetToken, string newPassword)
        //{
        //    var userId = GetUserIdFromPasswordToken(passwordResetToken.Trim());

        //    if (string.IsNullOrEmpty(userId))
        //        return false;

        //    //We have to remove the password before we can add it.
        //    var result = _userManager.RemovePassword(userId);

        //    if (!result.Succeeded)
        //        return false;

        //    //We have to add it because we do not have the old password to change it.
        //    result = _userManager.AddPassword(userId, newPassword.Trim());

        //    if (!result.Succeeded)
        //        return false;

        //    //Lets remove the token so it cannot be used again.
        //    RemovePasswordToken(userId);

        //    //TODO: Should use a timestamp on the token so the reset will not work after a set time.
        //    return true;
        //}

        //public string GetEmail(string userName)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    var user = GetUser(userName);

        //    var email = user?.Email;

        //    return email?.Trim();
        //}

        //public async Task<string> GetEmailAsync(string userName)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    var user = await GetUserAsync(userName);

        //    var email = user?.Email;

        //    return email?.Trim();
        //}


        //public bool ValidateUser(string userName, string password)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    if (string.IsNullOrEmpty(password))
        //        throw new ArgumentNullException(nameof(password));

        //    var user = _userManager.Find(userName.Trim(), password.Trim());

        //    return user != null && user.IsConfirmed && user.Active && !user.IsDeleted;
        //}

        //public async Task SignInAsync(ApplicationUser user, bool isPersistent)
        //{
        //    if (user == null)
        //        throw new ArgumentNullException(nameof(user));

        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

        //    var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

        //    AuthenticationManager.SignIn(new AuthenticationProperties {IsPersistent = isPersistent}, identity);
        //}

        //public async Task<bool> ConfirmAccountAsync(string accountConfirmationToken)
        //{
        //    if (string.IsNullOrEmpty(accountConfirmationToken))
        //        throw new ArgumentNullException(nameof(accountConfirmationToken));

        //    var user = await _unitOfWorkAsync.RepositoryAsync<ApplicationUser>()
        //        .Queryable()
        //        .FirstOrDefaultAsync(u => u.ConfirmationToken.Equals(accountConfirmationToken.Trim()));

        //    if (user == null) return false;

        //    user.IsConfirmed = true;
        //    user.TrackingState = TrackingState.Modified;

        //    _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Update(user);
        //    _unitOfWorkAsync.SaveChanges();

        //    return true;
        //}

        //public string CreateUserAndAccount(string userName, string password, string email,
        //    bool requireConfirmationToken = false)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));
        //    if (string.IsNullOrEmpty(password))
        //        throw new ArgumentNullException(nameof(password));
        //    if (string.IsNullOrEmpty(email))
        //        throw new ArgumentNullException(nameof(email));

        //    string token = null;
        //    if (requireConfirmationToken)
        //        token = ShortGuid.NewGuid();

        //    var isConfirmed = !requireConfirmationToken;

        //    if (token == null) return null;

        //    var user = new ApplicationUser
        //    {
        //        UserName = userName.Trim(),
        //        Email = email.Trim(),
        //        ConfirmationToken = token.Trim(),
        //        IsConfirmed = isConfirmed
        //    };

        //    user.TrackingState = TrackingState.Added;

        //    var result = _userManager.Create(user, password.Trim());
        //    if (result.Succeeded) return token;
        //    var innerMsg = new StringBuilder();
        //    foreach (var msg in result.Errors)
        //        innerMsg.Append(msg);
        //    var ex = new MembershipCreateUserException(innerMsg.ToString());
        //    throw ex;
        //}

        //public string CreateUserAndAccount(string userName, string password, string email, Guid myGuid,
        //    bool requireConfirmationToken = false)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));
        //    if (string.IsNullOrEmpty(password))
        //        throw new ArgumentNullException(nameof(password));
        //    if (string.IsNullOrEmpty(email))
        //        throw new ArgumentNullException(nameof(email));


        //    string token = null;
        //    if (requireConfirmationToken)
        //        token = ShortGuid.NewGuid();

        //    var isConfirmed = !requireConfirmationToken;

        //    if (token == null) return null;

        //    var user = new ApplicationUser
        //    {
        //        UserName = userName.Trim(),
        //        Email = email.Trim(),
        //        ConfirmationToken = token.Trim(),
        //        IsConfirmed = isConfirmed
        //    };

        //    user.TrackingState = TrackingState.Added;

        //    var result = _userManager.Create(user, password.Trim());
        //    if (result.Succeeded) return token;
        //    var innerMsg = new StringBuilder();
        //    foreach (var msg in result.Errors)
        //        innerMsg.Append(msg);
        //    var ex = new MembershipCreateUserException(innerMsg.ToString());
        //    throw ex;
        //}

        //public int? GetUserId(string userName)
        //{
        //    var user = _userManager.FindByName(userName.Trim());

        //    if (user == null) return null;
        //    var id = user.Id;

        //    return Convert.ToInt32(id);
        //}

        //public bool DeleteUser(string userName, bool deleteAllRelatedData)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    var user =
        //        _unitOfWorkAsync.RepositoryAsync<ApplicationUser>()
        //            .Queryable().SingleOrDefault(u => u.UserName.Equals(userName.Trim()));

        //    if (user == null)
        //        return false;

        //    user.TrackingState = TrackingState.Deleted;

        //    _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Delete(user.Id);

        //    _unitOfWorkAsync.SaveChanges();

        //    return true;
        //}

        //public async Task<bool> DeleteUserAsync(string userName, bool deleteAllRelatedData)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    var user =
        //        await _unitOfWorkAsync.RepositoryAsync<ApplicationUser>()
        //            .Queryable().SingleOrDefaultAsync(u => u.UserName.Equals(userName.Trim()));

        //    if (user == null)
        //        return false;

        //    user.TrackingState = TrackingState.Deleted;
        //    _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Delete(user.Id);

        //    await _unitOfWorkAsync.SaveChangesAsync();

        //    return true;
        //}

        //public string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 1440)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    var user =
        //        _unitOfWorkAsync.RepositoryAsync<ApplicationUser>()
        //            .Queryable()
        //            .SingleOrDefault(u => u.UserName.Equals(userName.Trim()));

        //    if (user == null)
        //        return string.Empty;

        //    string token = ShortGuid.NewGuid();

        //    user.PasswordResetToken = token;
        //    user.TrackingState = TrackingState.Modified;

        //    _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Update(user);
        //    _unitOfWorkAsync.SaveChanges();

        //    return token.Trim();
        //}

        //public async Task<string> GeneratePasswordResetTokenAsync(string userName,
        //    int tokenExpirationInMinutesFromNow = 1440)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    var user =
        //        await _unitOfWorkAsync.RepositoryAsync<ApplicationUser>()
        //            .Queryable()
        //            .SingleOrDefaultAsync(u => u.UserName.Equals(userName.Trim()));

        //    if (user == null)
        //        return string.Empty;

        //    string token = ShortGuid.NewGuid();

        //    user.PasswordResetToken = token;
        //    user.TrackingState = TrackingState.Modified;

        //    _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Update(user);
        //    await _unitOfWorkAsync.SaveChangesAsync();

        //    return token.Trim();
        //}

        //public async Task<string> GetUserIdFromPasswordTokenAsync(string passwordResetToken)
        //{
        //    if (string.IsNullOrEmpty(passwordResetToken))
        //        throw new ArgumentNullException(nameof(passwordResetToken));

        //    var user = await _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Queryable()
        //        .SingleOrDefaultAsync(u => u.PasswordResetToken.Equals(passwordResetToken.Trim()));

        //    var id = user?.Id;

        //    return id?.Trim();
        //}

        //public async void RemovePasswordTokenAsync(string userId)
        //{
        //    if (string.IsNullOrEmpty(userId))
        //        throw new ArgumentNullException(nameof(userId));

        //    var user = await _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Queryable()
        //        .SingleOrDefaultAsync(u => u.Id.Equals(userId.Trim()));

        //    if (user == null) return;

        //    user.PasswordResetToken = null;

        //    user.TrackingState = TrackingState.Modified;

        //    _unitOfWorkAsync.RepositoryAsync<ApplicationUser>().Update(user);


        //    await _unitOfWorkAsync.SaveChangesAsync();
        //}

        //public string GeneratePasswordResetTokenWithExpiration(
        //    string userName
        //    , int tokenExpirationInMinutesFromNow = 1440
        //)
        //{
        //    if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));
        //    if (tokenExpirationInMinutesFromNow <= 0)
        //        throw new ArgumentNullException(nameof(tokenExpirationInMinutesFromNow));

        //    var retVal = _userManager.GeneratePasswordResetToken(userName.Trim());

        //    return retVal?.Trim();
        //}

        //public string GetConfirmationToken(string userName)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    string token = null;

        //    var singleOrDefault = _unitOfWorkAsync
        //        .RepositoryAsync<ApplicationUser>()
        //        .Queryable()
        //        .SingleOrDefault(u => u.UserName.Equals(userName.Trim()));

        //    if (singleOrDefault == null) return null;
        //    token =
        //        singleOrDefault.ConfirmationToken.Trim();

        //    return token.Trim();
        //}

        //public async Task<string> GetConfirmationTokenAsync(string userName)
        //{
        //    if (string.IsNullOrEmpty(userName))
        //        throw new ArgumentNullException(nameof(userName));

        //    string token = null;

        //    var singleOrDefault = await _unitOfWorkAsync
        //        .RepositoryAsync<ApplicationUser>()
        //        .Queryable()
        //        .SingleOrDefaultAsync(u => u.UserName.Equals(userName.Trim()));

        //    if (singleOrDefault != null)
        //        token =
        //            singleOrDefault.ConfirmationToken;

        //    return token?.Trim();
        //}

        //public void MapUserToRole(string userId, string roleName)
        //{
        //    _userManager.AddToRole(userId.Trim(), roleName.Trim());
        //}

        //public Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo)
        //{
        //    return _userManager.FindAsync(loginInfo);
        //}

        //public Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        //{
        //    return AuthenticationManager.GetExternalLoginInfoAsync();
        //}

        //public Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string xsrfKey, string expectedValue)
        //{
        //    return AuthenticationManager.GetExternalLoginInfoAsync(xsrfKey.Trim(), expectedValue.Trim());
        //}

        //public IList<UserLoginInfo> GetLogins(string userId)
        //{
        //    return _userManager.GetLogins(userId.Trim());
        //}

        //public Task<string> GetPhoneNumberAsync(string userId)
        //{
        //    return _userManager.GetPhoneNumberAsync(userId.Trim());
        //}

        //public Task<bool> GetTwoFactorEnabledAsync(string userId)
        //{
        //    return _userManager.GetTwoFactorEnabledAsync(userId.Trim());
        //}

        //public Task<IList<UserLoginInfo>> GetLoginsAsync(string userId)
        //{
        //    return _userManager.GetLoginsAsync(userId.Trim());
        //}

        //public Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo login)
        //{
        //    return _userManager.RemoveLoginAsync(userId.Trim(), login);
        //}

        //public Task<ApplicationUser> FindByIdAsync(string userId)
        //{
        //    return _userManager.FindByIdAsync(userId.Trim());
        //}

        //public ApplicationUser FindById(string userId)
        //{
        //    return _userManager.FindById(userId.Trim());
        //}

        //public Task<string> GenerateChangePhoneNumberTokenAsync(string userId, string phoneNumber)
        //{
        //    return _userManager.GenerateChangePhoneNumberTokenAsync(userId.Trim(), phoneNumber.Trim());
        //}

        //public Task<IdentityResult> SetTwoFactorEnabledAsync(string userId, bool enabled)
        //{
        //    return _userManager.SetTwoFactorEnabledAsync(userId.Trim(), enabled);
        //}

        //public Task<IdentityResult> ChangePhoneNumberAsync(string userId, string phoneNumber, string token)
        //{
        //    return _userManager.ChangePhoneNumberAsync(userId.Trim(), phoneNumber.Trim(), token.Trim());
        //}

        //public Task<IdentityResult> SetPhoneNumberAsync(string userId, string phoneNumber)
        //{
        //    return _userManager.SetPhoneNumberAsync(userId.Trim(), phoneNumber.Trim());
        //}

        //public Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        //{
        //    return _userManager.ChangePasswordAsync(userId.Trim(), currentPassword.Trim(), newPassword.Trim());
        //}

        //public Task<IdentityResult> AddPasswordAsync(string userId, string newPassword)
        //{
        //    return _userManager.AddPasswordAsync(userId.Trim(), newPassword.Trim());
        //}

        //public Guid? GetUserProfileGuidByUserName(string userName)
        //{
        //    if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));

        //    var foundUser =
        //        _unitOfWorkAsync.RepositoryAsync<ApplicationUser>()
        //            .Queryable()
        //            .First(x => x.UserName.Trim()
        //                .Equals(userName.Trim()));

        //    return foundUser?.UserUid;
        //}


    }
}