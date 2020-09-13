using AutoMapper;
using IdentityProvider.Infrastructure.Logging.Serilog;
using IdentityProvider.Repository.EF.EFDataContext;
using IdentityProvider.Repository.EF.Queries.UserGrants;
using Logging.WCF.Models.Log4Net;
using StructureMap;
using System.Web.Mvc;


namespace IdentityProvider.Services.RowLeveLSecurityUserGrantService
{
    public class UserGrantService : IUserGrantService
    {
        private readonly ILog4NetLoggingService _loggingService;
        private readonly IMapper _mapper;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;

        [DefaultConstructor]  // This is the attribute you need to add on the constructor
        public UserGrantService(
             ILog4NetLoggingService loggingService
            , IMapper mapper
            , ApplicationSignInManager signInManager
            , ApplicationUserManager userManager
            , ApplicationRoleManager roleManager
        )
        {
            _loggingService = loggingService;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public GrantedPriviligesResponse OrgUnitGrantedPriviligesByUser(string userId)
        {
            var retVal = new GrantedPriviligesResponse();

            var context = (AppDbContext)DependencyResolver.Current.GetService(typeof(AppDbContext));
            var loggingFactory = (ISerilogLoggingFactory)DependencyResolver.Current.GetService(typeof(ISerilogLoggingFactory));

            var q = new UserGrantQuery(context, loggingFactory) { UserId = userId };

            var queryResponse = q.Execute();

            if (queryResponse.Succes)
            {
                retVal.Success = true;
                retVal.GrantedPriviliges = queryResponse.GrantedPriviliges;

                return retVal;
            }
            else
            {
                retVal.Message = queryResponse.Message;
            }

            return retVal;
        }

        public GrantedPriviligesResponse OrgUnitGrantedPriviligesByEmployee(int employeeId)
        {
            var retVal = new GrantedPriviligesResponse();

            var context = (AppDbContext)DependencyResolver.Current.GetService(typeof(AppDbContext));
            var loggingFactory = (ISerilogLoggingFactory)DependencyResolver.Current.GetService(typeof(ISerilogLoggingFactory));

            var q = new UserGrantQuery(context, loggingFactory) { EmployeeId = employeeId };

            var queryResponse = q.Execute();

            if (queryResponse.Succes)
            {
                retVal.Success = true;
                retVal.GrantedPriviliges = queryResponse.GrantedPriviliges;

                return retVal;
            }
            else
            {
                retVal.Message = queryResponse.Message;
            }

            return retVal;
        }
    }
}