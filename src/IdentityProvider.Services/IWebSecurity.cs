using IdentityProvider.Models.Domain.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityProvider.Services
{
    public interface IWebSecurity
    {
        List<ApplicationUser> UsersActiveGetAll();

        #region Identity 2.0

        Task<ApplicationUser> GetUserByUserNameAsync(string username);
        int? UpdateUser(ApplicationUser user);

        Task<SignInStatus> PasswordSignInAsync(string modelEmail, string modelPassword, bool modelRememberMe,
            bool shouldLockout);

        Task<bool> HasBeenVerifiedAsync();

        Task<SignInStatus> TwoFactorSignInAsync(string modelProvider, string modelCode, bool isPersistent,
            bool rememberBrowser);

        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task SignInAsync(ApplicationUser user, bool isPersistent, bool rememberBrowser);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
        Task<ApplicationUser> FindByNameAsync(string modelEmail);
        Task<bool> IsEmailConfirmedAsync(string id);
        Task<IdentityResult> ResetPasswordAsync(string id, string modelCode, string modelPassword);
        Task<string> GetVerifiedUserIdAsync();
        Task<IList<string>> GetValidTwoFactorProvidersAsync(string userId);
        Task<bool> SendTwoFactorCodeAsync(string modelSelectedProvider);
        Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo, bool isPersistent);
        Task<IdentityResult> AddLoginAsync(string id, UserLoginInfo login);
        Task<IdentityResult> CreateAsync(ApplicationUser user);

        #endregion cdentity 2.0
    }
}