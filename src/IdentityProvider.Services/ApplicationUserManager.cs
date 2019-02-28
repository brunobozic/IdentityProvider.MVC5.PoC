using System;
using IdentityProvider.Infrastructure.Email;
using IdentityProvider.Infrastructure.SMS;
using IdentityProvider.Models.Domain.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace IdentityProvider.Services
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly IEmailService _emailService;
        private readonly DpapiDataProtectionProvider _provider = new DpapiDataProtectionProvider("IdentityProvider");

        public ApplicationUserManager(
            IUserStore<ApplicationUser> store
            , IEmailService emailService
            , IIdentityMessageService identityEmailMessageService
            ) : base(store)
        {
            _emailService = emailService;

            // Configure validation logic for usernames
            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false ,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8 ,
                RequireNonLetterOrDigit = true ,
                RequireDigit = true ,
                RequireLowercase = true ,
                RequireUppercase = true
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code" , new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });

            RegisterTwoFactorProvider("Email Code" , new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code" ,
                BodyFormat = "Your security code is {0}"
            });

            EmailService = identityEmailMessageService;
            SmsService = new SmsService();

            UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(_provider.Create("ASP.NET Identity"));

            //return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>())
            //{
            //    UserTokenProvider =
            //        new DataProtectorTokenProvider<ApplicationUser , string>(_provider.Create("ASP.NET Identity"))
            //};


        }

        //public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
        //    IOwinContext context)
        //{
        //    var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<DataContextAsync>()));

        //    // Configure validation logic for usernames
        //    manager.UserValidator = new UserValidator<ApplicationUser>(manager)
        //    {
        //        AllowOnlyAlphanumericUserNames = false,
        //        RequireUniqueEmail = true
        //    };

        //    // Configure validation logic for passwords
        //    manager.PasswordValidator = new PasswordValidator
        //    {
        //        RequiredLength = 8,
        //        RequireNonLetterOrDigit = true,
        //        RequireDigit = true,
        //        RequireLowercase = true,
        //        RequireUppercase = true,
        //    };

        //    // Configure user lockout defaults
        //    manager.UserLockoutEnabledByDefault = true;
        //    manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
        //    manager.MaxFailedAccessAttemptsBeforeLockout = 5;

        //    // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
        //    // You can write your own provider and plug it in here.
        //    manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
        //    {
        //        MessageFormat = "Your security code is {0}"
        //    });
        //    manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
        //    {
        //        Subject = "Security Code",
        //        BodyFormat = "Your security code is {0}"
        //    });
        //    manager.EmailService = new EmailService();
        //    manager.SmsService = new SmsService();
        //    var dataProtectionProvider = options.DataProtectionProvider;
        //    if (dataProtectionProvider != null)
        //    {
        //        manager.UserTokenProvider =
        //            new DataProtectorTokenProvider<ApplicationUser>(
        //                dataProtectionProvider.Create("ASP.NET Identity"));
        //    }
        //    return manager;
        //}
    }
}