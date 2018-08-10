using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web.Security;
using HAC.Helpdesk.Infrastructure.ConfigurationFront.Implementation;
using HAC.Helpdesk.Infrastructure.ConfigurationFront.Interface;
using HAC.Helpdesk.Infrastructure.DatabaseLog;
using HAC.Helpdesk.Infrastructure.Log4Net.Implementation;
using HAC.Helpdesk.Infrastructure.LoggingFront.Implementation;
using HAC.Helpdesk.Infrastructure.LoggingFront.Interface;
using HAC.Helpdesk.SimpleMembership.Repository.EF.DataContexts;
using HAC.Helpdesk.SimpleMembership.Repository.EF.UnitOfWork;
using HAC.Helpdesk.SimpleMembership.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HAC.Helpdesk.SimpleMembership.Repository.EF.Test
{
    [TestClass]
    public class SimpleMembershipServiceIntegrationTest
    {
        private ISimpleMembershipDataContextAsync _dataContextAsync;
        private IConfigurationRepository _fakeConfigurationRepository;
        private IContextService _fakeContextService;
        private IWcfAppenderService _fakewcfAppenderService;
        private ILoggingService _loggingService;
        private IWebSecurity _simpleMembershipService;
        private ISimpleMembershipUnitOfWorkAsync _uow;
        private readonly List<string> _createdTestUsers = new List<string>();

        private List<string> CreatedTestUsers => _createdTestUsers;

        [TestInitialize]
        public void TestInitialize()
        {
            // Not fake...
            _dataContextAsync = new SimpleMembershipDataContextAsync();

            // Is fake...
            _fakeConfigurationRepository = new FakeConfigFileConfigurationRepository();
            _fakeConfigurationRepository.FakeSettingsDictionary.Clear();
            _fakeConfigurationRepository.FakeSettingsDictionary.Add("Log4NetSettingsFile", "Log4Net.config.xml");
            _fakeConfigurationRepository.FakeSettingsDictionary.Add("LogEverythingViaWCF", "false");
            _fakeConfigurationRepository.FakeSettingsDictionary.Add("DontLogAnythingViaWCF", "false");
            _fakeConfigurationRepository.FakeSettingsDictionary.Add("LogEverythingToFile", "true");
            _fakeConfigurationRepository.FakeSettingsDictionary.Add("LoggingServiceURL",
                "http://localhost:63247/LogWCF.svc");

            var dir = Directory.GetCurrentDirectory();
            var resourceFileInfo = new FileInfo(Path.Combine(dir, "Log4Net.config.xml"));

            // Is fake...
            _fakeContextService = new FakeThreadContextService
            {
                FakeGetContextualFullFilePath = resourceFileInfo.FullName,
                FakeUserNameForTestingPurposes = "SimpleMembershipServiceUnitTest"
            };

            // Is fake...
            _fakewcfAppenderService = new FakeWcfAppenderService(_fakeConfigurationRepository);


            // Is fake...
            _loggingService = new Log4NetLoggingService(_fakeConfigurationRepository, _fakeContextService,
                _fakewcfAppenderService, true);

            // Not fake...
            _uow = new SimpleMembershipUnitOfWorkAsync(_dataContextAsync, _loggingService);

            // Not fake...
            _simpleMembershipService = new WebSecurity(_uow, _loggingService);


            InitDb();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // cleanup all the infrastructure that was needed for our tests.

            if (_dataContextAsync != null)
            {
                _dataContextAsync.Dispose();
            }

            if (_fakeConfigurationRepository != null)
            {
                _fakeConfigurationRepository.Dispose();
            }

            if (_fakeContextService != null)
            {
                _fakeContextService.Dispose();
            }
            // _fakewcfAppenderService.
            // _logManager

            if (_loggingService != null)
            {
                _loggingService.Dispose();
            }

            if (_uow != null)
            {
                _uow.Dispose();
            }

            _simpleMembershipService = null;
        }

        [TestMethod]
        public void Can_Get_User_By_UserName_Expecting_UserName()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            const string username = "GetByName";
            const string email = "GetByName@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ] was *null*. ");
            }

            var actualUser = _simpleMembershipService.GetUserByUserName(username);

            // Expecting to find the user entity that has the requested user name...
            Assert.AreEqual(username, actualUser.UserName,
                "User names do not match. Expected: [ " + username + " ], but instead found: [ " +
                actualUser.UserName.Trim() +
                " ]. ");
        }

        [TestMethod]
        [ExpectedException(typeof(MembershipCreateUserException), "The username is already in use")]
        public void Creating_Already_Existing_User_Throws_Exception()
        {
            var variableToken = DateTime.Now.ToLongTimeString();
            string token;
            const string username = "testIvanIvic";
            const string email = "iivic@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";

            if (!_simpleMembershipService.FoundUser(username))
            {
                // The user will now be created...
                token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ] was *null*. ");
            }

            // We expect this to fail as the same user cannot be created twice...
            token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
            Assert.IsNull(token, "Token for selected username: [ " + username + " ] was *not* *null*. ");
        }

        [TestMethod]
        public void Can_Get_Confirmation_Token_For_UserName_Expecting_Token()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            const string username = "testIvanIvic";
            const string email = "iivic@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";

            _uow = new SimpleMembershipUnitOfWorkAsync(_dataContextAsync, _loggingService);
            _simpleMembershipService = new WebSecurity(_uow, _loggingService);

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ]  was *null*. ");
            }

            var actualToken = _simpleMembershipService.GetConfirmationToken(username);

            // The confirmation token should be generated...
            Assert.IsNotNull(actualToken, "Token for requested username: [ " + username + " ] is *null*. ");
        }

        [TestMethod]
        public void Can_Validate_User_When_User_Has_NOT_Confirmed_His_Account_Expecting_False()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            var username = "ValidateUserFail" + variableToken;
            const string email = "ValidateUserFail@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ]  was *null*. ");
            }

            var userIsValidated = _simpleMembershipService.ValidateUser(username, pwd);

            // User should not be validate as he hasnt confirmed his account using the confirmation token...
            Assert.IsFalse(userIsValidated,
                "Users account was *not* confirmed, therefore we expected validation result to be [ false ]. ");
        }

        [TestMethod]
        public void Can_Confirm_Account_Using_Token_Recieved_While_Registering_Expecting_True()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            var username = "ConfirmAccount" + variableToken;
            const string email = "ConfirmAccount@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";

            _uow = new SimpleMembershipUnitOfWorkAsync(_dataContextAsync, _loggingService);
            _simpleMembershipService = new WebSecurity(_uow, _loggingService);

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ] was *null*. ");
            }

            // Getting the confirmation token...
            var actualToken = _simpleMembershipService.GetConfirmationToken(username);

            // Passing the confirmation token and confirming the account...
            // We expect the user to be confirmed
            var isConfirmed = _simpleMembershipService.ConfirmAccount(actualToken);

            Assert.IsTrue(isConfirmed, "The requested users acount: [ " + username + " ] could *not* be confirmed. ");
        }

        [TestMethod]
        public void Can_Validate_User_When_User_Has_Confirmed_His_Account_Expected_True()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            var username = "ValidateUser" + variableToken;
            const string email = "ValidateUser@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ] was *null*. ");

                var actualToken = _simpleMembershipService.GetConfirmationToken(username);

                // Setting up the test - the users account must be confirmed for this test.
                var isConfirmed = _simpleMembershipService.ConfirmAccount(actualToken);
            }

            // We expect the user to be validated as he had already confirmed his account using the generated confirmation token...
            var userIsValidated = _simpleMembershipService.ValidateUser(username, pwd);

            Assert.IsTrue(userIsValidated,
                "Users account: [ " + username + " ] was *not* validated even after being confirmed. ");
        }

        [TestMethod]
        public void Can_Delete_User_Expected_True()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            const string username = "DeleteUser";
            const string email = "DeleteUser@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";
            var token = false;

            if (_simpleMembershipService.FoundUser(username))
            {
                token = _simpleMembershipService.DeleteUser(username, true);
            }

            // We expect the user to be deleted...
            Assert.IsTrue(token, "The selected user: [ " + username + " ] could not be deleted. ");
        }

        [TestMethod]
        public void
            Can_Generate_Password_Retrieve_Token_That_Expires_In_1_Minute_And_Reset_Password_Within_Time_Limit_Expected_True
            ()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            var username = "PwdResetSuccess" + variableToken;
            const string email = "PwdResetSuccess@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";

            _uow = new SimpleMembershipUnitOfWorkAsync(_dataContextAsync, _loggingService);
            _simpleMembershipService = new WebSecurity(_uow, _loggingService);

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ] was *null*. ");
            }

            var actualToken = _simpleMembershipService.GetConfirmationToken(username);
            Assert.IsNotNull(actualToken, "Confirmation token for selected username: [ " + username + " ] was *null*. ");

            var isConfirmed = _simpleMembershipService.ConfirmAccount(actualToken);
            Assert.IsTrue(isConfirmed, "The requested users acount: [ " + username + " ] could *not* be confirmed. ");

            // We expect the account to already be confirmed at this point...
            var passwordResetToken = _simpleMembershipService.GeneratePasswordResetToken(username, 1);
            Assert.IsTrue(passwordResetToken.Length > 1, "Password reset token was *not* generated. ");

            // We expect the password to be successfully reset as the timeout period has *not* timed out...
            var passwordWasReset = _simpleMembershipService.ResetPassword(passwordResetToken, "newPassword");
            Assert.IsTrue(passwordWasReset, "Password was *not* reset. ");
        }

        [TestMethod]
        public void
            Can_Generate_Password_Retrieve_Token_That_Expires_In_1_Minute_And_Reset_Password_After_Time_Limit_Expected_Fail
            ()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            var username = "PwdResetFail" + variableToken;
            const string email = "PwdResetFail@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";

            _uow = new SimpleMembershipUnitOfWorkAsync(_dataContextAsync, _loggingService);
            _simpleMembershipService = new WebSecurity(_uow, _loggingService);

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ] was *null*. ");
            }

            var actualToken = _simpleMembershipService.GetConfirmationToken(username);
            Assert.IsNotNull(actualToken, "Confirmation token for selected username: [ " + username + " ] was *null*. ");

            var isConfirmed = _simpleMembershipService.ConfirmAccount(actualToken);
            Assert.IsTrue(isConfirmed, "The requested users acount: [ " + username + " ] could *not* be confirmed. ");

            // We expect the account to already be confirmed at this point...
            var passwordResetToken = _simpleMembershipService.GeneratePasswordResetToken(username, 1);
            Assert.IsTrue(passwordResetToken.Length > 1, "Password reset token was *not* generated. ");

            Thread.Sleep(65000); // Minute and a half...

            // We expect the password reset to fail due to the timeout period expiry...
            var passwordWasReset = _simpleMembershipService.ResetPassword(passwordResetToken, "newPassword");
            Assert.IsFalse(passwordWasReset, "Password should *not* have been reset - due to timeout. ");
        }

        [TestMethod]
        public void Can_Get_Current_User_Expecting_Current_User()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            var username = "GetCurrentUser";
            const string email = "GetCurrentUser@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";

            _uow = new SimpleMembershipUnitOfWorkAsync(_dataContextAsync, _loggingService);
            _simpleMembershipService = new WebSecurity(_uow, _loggingService);

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ] was *null*. ");
            }

            // TODO: needs to mock the context in order to test this...
            // Context.User.Identity.Name

            // var currentUser = _simpleMembershipService.GetCurrentUser();
        }

        [TestMethod]
        public void Can_Get_Users_Guid_By_UserName_Expecting_Valid_Guid()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            var myGuid = Guid.NewGuid();
            var username = "UserNameGetGuid" + variableToken;
            const string email = "UserNameGetGuid@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";

            _uow = new SimpleMembershipUnitOfWorkAsync(_dataContextAsync, _loggingService);
            _simpleMembershipService = new WebSecurity(_uow, _loggingService);

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, myGuid, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ] was *null*. ");
            }

            var foundGuid = _simpleMembershipService.GetUserProfileGuidByUserName(username);

            // We expect to get a valid user Guid...
            Assert.IsTrue(foundGuid.HasValue, "No Guid was found. ");
            Assert.IsTrue(foundGuid.Value.ToString().Length > 0, "Length of found Guid was 0. ");
            Assert.AreEqual(myGuid, foundGuid.Value,
                "Guids do not match. Expected: [ " + myGuid + " ], but instead found: [ " + foundGuid.Value + " ]. ");
        }

        [TestMethod]
        public void Can_Get_Email_By_UserName_Expecting_Valid_Email()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            const string username = "GetEmail";
            const string email = "PasswordToken@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";

            _uow = new SimpleMembershipUnitOfWorkAsync(_dataContextAsync, _loggingService);
            _simpleMembershipService = new WebSecurity(_uow, _loggingService);

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ] was *null*. ");
            }

            var actualEmail = _simpleMembershipService.GetEmail(username);

            // We expect to get a valid email address...
            Assert.AreEqual(actualEmail, email,
                "Emails do not match. Expected: [ " + email + " ], but instead found: [ " + actualEmail.Trim() + " ]. ");
        }

        [TestMethod]
        public void Can_Update_User_By_Updating_The_UserName_Property_Expecting_Success()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            var username = "UserNameBeforeUpd";
            const string email = "UserNameUpd@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "password";
            const string updatedUserName = "UpdatedUserName";

            _uow = new SimpleMembershipUnitOfWorkAsync(_dataContextAsync, _loggingService);
            _simpleMembershipService = new WebSecurity(_uow, _loggingService);

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ] was *null*. ");
            }

            var userProfileOriginal = _simpleMembershipService.GetUserByUserName(username);
            var originalOne = userProfileOriginal.RowVersion;
            Assert.IsNotNull(userProfileOriginal,
                "Could *not* find the user account by user name: [ " + username + " ], before the update. ");


            userProfileOriginal.UserName = updatedUserName;
            var isUpdated = _simpleMembershipService.UpdateUser(userProfileOriginal);

            Assert.IsNotNull(isUpdated, "The user: [ " + userProfileOriginal.UserName + " ] was *not* updated. ");

            var userProfileAfterTheUpdate = _simpleMembershipService.GetUserByUserName(updatedUserName);

            // We expect to find that this users enitity was indeed updated...
            Assert.IsNotNull(userProfileAfterTheUpdate,
                "Could *not* find the user account by user name: [ " + updatedUserName + " ] after the update. ");
            Assert.AreEqual(updatedUserName, userProfileAfterTheUpdate.UserName,
                "User Names after update do *not* match. Expected: [ " + updatedUserName + "], but instead found: [ " +
                userProfileAfterTheUpdate.UserName.Trim() + " ]. ");
        }

        [TestMethod]
        public void Can_Login()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            const string username = "UserNameLogin";
            const string email = "Login@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "login";

            _uow = new SimpleMembershipUnitOfWorkAsync(_dataContextAsync, _loggingService);
            _simpleMembershipService = new WebSecurity(_uow, _loggingService);

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ] was *null*. ");
            }

            var hasLoggedIn = _simpleMembershipService.Login(username, pwd);

            Assert.IsTrue(hasLoggedIn,
                "The user login for: [ " + username + " ] was *not* successfull. ");
        }

        [TestMethod]
        public void Can_Logout()
        {
            var variableToken = DateTime.Now.ToLongTimeString();

            const string username = "UserNameLogout";
            const string email = "Logout@gmail.com";
            const string woFs = "myTwoFactorSecret";
            const string pwd = "logout";

            _uow = new SimpleMembershipUnitOfWorkAsync(_dataContextAsync, _loggingService);
            _simpleMembershipService = new WebSecurity(_uow, _loggingService);

            if (!_simpleMembershipService.FoundUser(username))
            {
                var token = _simpleMembershipService.CreateUserAndAccount(username, pwd, email, true);
                CreatedTestUsers.Add(username);
                Assert.IsNotNull(token, "Token for selected username: [ " + username + " ] was *null*. ");
            }

            _simpleMembershipService.Logout();

            // TODO: how to test user logging out???? need to mock HttpContext?
        }

        private static void InitDb()
        {
        }
    }
}