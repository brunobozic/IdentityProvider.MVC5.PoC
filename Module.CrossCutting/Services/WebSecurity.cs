using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using IdentityProvider.Infrastructure;
using IdentityProvider.Infrastructure.ApplicationContext;
using IdentityProvider.Models.Domain.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TrackableEntities;
using URF.Core.EF;

namespace IdentityProvider.Services
{
    public class WebSecurity : IWebSecurity, IDisposable
    {
        private readonly ICachedUserAuthorizationGrantsProvider _cachedUserAuthorizationGrantsProvider;
        private readonly IMapper _mapper;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        //private readonly ILog4NetLoggingService _loggingService;
        private ApplicationUserManager _userManager;

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
                // FetchLoggerAndLog(ex);
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

        #region Ctor

        public WebSecurity(
            IUnitOfWorkAsync unitOfWorkAsync
            //, ILog4NetLoggingService loggingService
            , IMapper mapper
            , ApplicationSignInManager signInManager
            , ApplicationUserManager userManager
            , ICachedUserAuthorizationGrantsProvider cachedUserAuthorizationGrantsProvider
        )
        {
            _userManager = userManager;
            _cachedUserAuthorizationGrantsProvider = cachedUserAuthorizationGrantsProvider;
            _signInManager = signInManager;
            //_loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWorkAsync = unitOfWorkAsync ?? throw new ArgumentNullException(nameof(unitOfWorkAsync));
            // _userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }


        public WebSecurity(
            IContextProvider contextService
          
            , ApplicationUserManager userManager
            , ApplicationSignInManager signInManager
        )
        {
           
            _userManager = userManager;
            _signInManager = signInManager;

            try
            {
                var dbContextAsync = DataContextFactory.GetDataContextAsync();
                dbContextAsync.GetDatabase().Initialize(true);


                _unitOfWorkAsync = new UnitOfWork(dbContextAsync,
                    new RowAuthPoliciesContainer(_cachedUserAuthorizationGrantsProvider));
                // UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dbContextAsync as DbContext));
            }
            catch (Exception ex)
            {
                // FetchLoggerAndLog(ex);
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
                    // FetchLoggerAndLog(disposeException);

                    //_loggingService.LogWarning(this, "IDisposable problem disposing", disposeException);
                }
        }

        #endregion Dispose


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
    }
}