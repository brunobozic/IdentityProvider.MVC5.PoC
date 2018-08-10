using System.IO;
using AutoMapper;
using HAC.Helpdesk.Infrastructure.ConfigurationFront.Implementation;
using HAC.Helpdesk.Infrastructure.DatabaseLog;
using HAC.Helpdesk.Infrastructure.Log4Net.Implementation;
using HAC.Helpdesk.Infrastructure.LoggingFront.Implementation;
using HAC.Helpdesk.Services.Implementations;
using HAC.Helpdesk.Services.MessagePattern.UserProfileAdministration;
using HAC.Helpdesk.SimpleMembership.Repository.EF.DataContexts;
using HAC.Helpdesk.SimpleMembership.Repository.EF.UnitOfWork;
using HAC.Helpdesk.SimpleMembership.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HAC.Helpdesk.SimpleMembership.Repository.EF.Test
{
    [TestClass]
    public class UserProfileAdministrationServiceIntegrationTest
    {
        private SimpleMembershipDataContextAsync _dataContextAsync;
        private FakeConfigFileConfigurationRepository _fakeConfigurationRepository;
        private FakeThreadContextService _fakeContextService;
        private FakeWcfAppenderService _fakewcfAppenderService;
        private Log4NetLoggingService _loggingService;
        private IMapper _mapper;
        private SimpleMembershipUnitOfWorkAsync _uow;
        private IWebSecurity MembershipService { get; set; }

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

         
            // Not fake...
            _loggingService = new Log4NetLoggingService(_fakeConfigurationRepository, _fakeContextService,
                _fakewcfAppenderService, true);

            // Not fake...
            _uow = new SimpleMembershipUnitOfWorkAsync(_dataContextAsync, _loggingService);

            // Not fake...
            MembershipService = new WebSecurity(_uow, _loggingService);


            InitDb();
        }

        private static void InitDb()
        {
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

            MembershipService = null;
        }

        [TestMethod]
        public void Can_Get_All_Active_User_Profiles_As_Paged_List_Single_Page_With_5_Items_Expecting_Success()
        {
            const int myPageSizeSetting = 5;

            var myRequest = new UserProfileGetAllActiveRequest {Active = true, Page = 1, PageSize = 5};

            var serviceToTest = new UserProfileAdministrationService(_uow, _loggingService, _mapper);

            var myResponse = serviceToTest.UserProfileGetAllActive(myRequest);

            Assert.IsTrue(myResponse.Success);
            Assert.IsTrue(myResponse.ActiveUsersList.Count > 0, "No items were found.");
            Assert.IsTrue(myResponse.ActiveUsersList.Count == myPageSizeSetting, "Page size setting was not evaluated.");
        }

        //    Assert.IsTrue(myResponse.Success);

        //    myResponse = serviceToTest.UserProfileGetAllActive(myRequest);

        //    AutoMapperBootStrapper.ConfigureAutoMapper();
        //    var serviceToTest = new UserProfileAdministrationService(_uow, _loggingService);
        //    UserProfileGetAllActiveResponse myResponse;

        //    var myRequest = new UserProfileGetAllActiveRequest {Active = true, Page = 1, PageSize = 2};
        //    const int myPageSizeSetting = 5;
        //{
        //    ()
        //    Can_Get_All_Active_User_Profiles_As_Paged_List_Single_Page_With_2_Items_Then_We_Go_To_Page_2_Showing_2_More_Records_Expecting_Success
        //public void

        //[TestMethod]
        //    Assert.IsTrue(myResponse.ActiveUsersList.Count > 0, "No items were found.");
        //    Assert.IsTrue(myResponse.ActiveUsersList.Count == myPageSizeSetting, "Page size setting was not evaluated.");
        //}

        //[TestMethod]
        //public void
        //    UserProfileChangeEmailForUserName
        //    ()
        //{
        //    const int myPageSizeSetting = 5;

        //    var myRequest = new UserProfileGetAllActiveRequest {Active = true, Page = 1, PageSize = 2};
        //    UserProfileGetAllActiveResponse myResponse;
        //    var serviceToTest = new UserProfileAdministrationService(_uow, _loggingService);

        //    AutoMapperBootStrapper.ConfigureAutoMapper();

        //    myResponse = serviceToTest.UserProfileGetAllActive(myRequest);

        //    Assert.IsTrue(myResponse.Success);
        //    Assert.IsTrue(myResponse.ActiveUsersList.Count > 0, "No items were found.");
        //    Assert.IsTrue(myResponse.ActiveUsersList.Count == myPageSizeSetting, "Page size setting was not evaluated.");
        //}

        //[TestMethod]
        //public void
        //    UserProfileChangeMobilePhoneForUserName
        //    ()
        //{
        //    const int myPageSizeSetting = 5;

        //    var myRequest = new UserProfileGetAllActiveRequest {Active = true, Page = 1, PageSize = 2};
        //    UserProfileGetAllActiveResponse myResponse;
        //    var serviceToTest = new UserProfileAdministrationService(_uow, _loggingService);

        //    AutoMapperBootStrapper.ConfigureAutoMapper();

        //    myResponse = serviceToTest.UserProfileGetAllActive(myRequest);

        //    Assert.IsTrue(myResponse.Success);
        //    Assert.IsTrue(myResponse.ActiveUsersList.Count > 0, "No items were found.");
        //    Assert.IsTrue(myResponse.ActiveUsersList.Count == myPageSizeSetting, "Page size setting was not evaluated.");
        //}

        //[TestMethod]
        //public void
        //    UserProfileChangeHomePhoneForUserName
        //    ()
        //{
        //    const int myPageSizeSetting = 5;

        //    var myRequest = new UserProfileGetAllActiveRequest {Active = true, Page = 1, PageSize = 2};
        //    UserProfileGetAllActiveResponse myResponse;
        //    var serviceToTest = new UserProfileAdministrationService(_uow, _loggingService);

        //    AutoMapperBootStrapper.ConfigureAutoMapper();

        //    myResponse = serviceToTest.UserProfileGetAllActive(myRequest);

        //    Assert.IsTrue(myResponse.Success);
        //    Assert.IsTrue(myResponse.ActiveUsersList.Count > 0, "No items were found.");
        //    Assert.IsTrue(myResponse.ActiveUsersList.Count == myPageSizeSetting, "Page size setting was not evaluated.");
        //}

        //[TestMethod]
        //public void
        //    UserProfileGetAllActiveByUserNameAsync
        //    ()
        //{
        //    const int myPageSizeSetting = 5;

        //    var myRequest = new UserProfileGetAllActiveRequest {Active = true, Page = 1, PageSize = 2};
        //    UserProfileGetAllActiveResponse myResponse;
        //    var serviceToTest = new UserProfileAdministrationService(_uow, _loggingService);

        //    AutoMapperBootStrapper.ConfigureAutoMapper();

        //    myResponse = serviceToTest.UserProfileGetAllActive(myRequest);

        //    Assert.IsTrue(myResponse.Success);
        //    Assert.IsTrue(myResponse.ActiveUsersList.Count > 0, "No items were found.");
        //    Assert.IsTrue(myResponse.ActiveUsersList.Count == myPageSizeSetting, "Page size setting was not evaluated.");
        //}
    }
}